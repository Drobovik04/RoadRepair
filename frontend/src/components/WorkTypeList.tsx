import React, { useEffect, useState } from "react";
import {
  Button,
  Card,
  DatePicker,
  Divider,
  Form,
  Input,
  List,
  Modal,
  Popconfirm,
  Select,
  Space,
  message,
  Typography,
} from "antd";
const { Title, Text } = Typography;
import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import {
  getRepairEventsByRepairZoneId,
  addRepairEvent,
  updateRepairEvent,
  deleteRepairEvent,
} from "../services/repairEvents";
import type { RepairEvent } from "../types/RepairEvent";
import type { TypeOfRepair } from "../types/TypeOfRepair";
import { getTypesOfRepair } from "../services/typesOfRepair";
import { normalize } from "../utilities/dayjsStringConverter";
import MaterialList from "./MaterialList";
import MediaList from "./MediaList";
import WorkTimeTable from "./WorkTImeTable";
import { colorSchemes } from "../utilities/colors";

interface Props {
  zoneId: number;
  onWorkTypeUpdated: () => void;
}

const WorkTypeList = ({ zoneId, onWorkTypeUpdated }: Props) => {
  const [typesOfRepair, setTypesOfRepair] = useState<TypeOfRepair[]>([]);
  const [repairEvents, setRepairEvents] = useState<RepairEvent[]>([]);
  const [modalVisible, setModalVisible] = useState(false);
  const [form] = Form.useForm();
  const [editingRepairEvent, setEditingRepairEvent] =
    useState<RepairEvent | null>(null);

  useEffect(() => {
    getTypesOfRepair().then((res) => setTypesOfRepair(res.data));
    loadWorks();
  }, [zoneId]);

  const loadWorks = async () => {
    try {
      const res = await getRepairEventsByRepairZoneId(zoneId);
      setRepairEvents(res.data);
    } catch {
      message.error("Ошибка загрузки видов работ");
    }
  };

  const openCreateModal = () => {
    form.resetFields();
    form.setFieldValue("repairZoneId", zoneId);
    setEditingRepairEvent(null);
    setModalVisible(true);
  };

  const openEditModal = (event: RepairEvent) => {
    const normalized = normalize(event, ["startedAt", "endedAt"]);
    form.setFieldsValue(normalized);
    setEditingRepairEvent(event);
    setModalVisible(true);
  };
  const handleSubmit = async (values: any) => {
    try {
      const payload = {
        ...values,
        repairZoneId: zoneId,
        startedAt: values.startedAt?.format("YYYY-MM-DD"),
        endedAt: values.endedAt?.format("YYYY-MM-DD"),
      };

      if (editingRepairEvent) {
        await updateRepairEvent(editingRepairEvent.id, payload);
        message.success("Вид работ обновлен");
      } else {
        await addRepairEvent(payload);
        message.success("Вид работ добавлен");
      }

      form.resetFields();
      setModalVisible(false);
      setEditingRepairEvent(null);
      loadWorks();
      onWorkTypeUpdated();
    } catch (err) {
      message.error("Ошибка при сохранении вида работ");
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteRepairEvent(id);
      message.success("Удалено");
      loadWorks();
      onWorkTypeUpdated();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  return (
    <>
      <Title level={4}>Работы на участках</Title>
      <Space style={{ marginBottom: 16 }}>
        <Button icon={<PlusOutlined />} onClick={openCreateModal}>
          Добавить вид работ
        </Button>
      </Space>
      <List
        bordered
        dataSource={repairEvents}
        renderItem={(item, index) => {
          const scheme = colorSchemes[2];
          const scheme1 = colorSchemes[3];
          return (
            //
            <List.Item
              style={{ backgroundColor: scheme.head, borderRadius: 8 }}
            >
              <Card
                title={
                  <Title level={4} style={{ margin: 0 }}>
                    {item.typeOfRepairName}
                  </Title>
                }
                extra={
                  <Space>
                    <Button
                      icon={<EditOutlined />}
                      onClick={() => openEditModal(item)}
                    >
                      Редактировать
                    </Button>
                    <Popconfirm
                      title="Удалить?"
                      okText="ОК"
                      cancelText="Отмена"
                      onConfirm={() => handleDelete(item.id)}
                    >
                      <Button danger icon={<DeleteOutlined />}>
                        Удалить
                      </Button>
                    </Popconfirm>
                  </Space>
                }
                style={{ width: "100%" }}
                headStyle={{ backgroundColor: scheme1.head, borderWidth: 2 }}
                bodyStyle={{ backgroundColor: scheme1.body }}
              >
                <Text>
                  Дата проведения работ: {item.startedAt?.toString()} —{" "}
                  {item.endedAt?.toString()}
                </Text>
                <MaterialList
                  repairEventId={item.id}
                  onMaterialListUpdated={onWorkTypeUpdated}
                />
                <Divider />
                <MediaList repairEventId={item.id} />
              </Card>
            </List.Item>
          );
        }}
      />

      <Modal
        title="Добавить вид работ"
        open={modalVisible}
        onCancel={() => {
          form.resetFields();
          setEditingRepairEvent(null);
          setModalVisible(false);
        }}
        onOk={() => form.validateFields().then(handleSubmit)}
        okText={editingRepairEvent ? "Сохранить" : "Добавить"}
        cancelText="Отмена"
        destroyOnHidden
      >
        <Form form={form} layout="vertical">
          <Form.Item name="repairZoneId" hidden>
            <Input />
          </Form.Item>
          <Form.Item
            name="typeOfRepairId"
            label="Вид работы"
            rules={[{ required: true }]}
          >
            <Select>
              {typesOfRepair.map((u) => (
                <Select.Option key={u.id} value={u.id}>
                  {u.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item
            name="startedAt"
            label="Начало работы"
            rules={[{ required: true }]}
          >
            <DatePicker format="YYYY-MM-DD" />
          </Form.Item>
          <Form.Item name="endedAt" label="Окончание работы">
            <DatePicker format="YYYY-MM-DD" />
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
};

export default WorkTypeList;
