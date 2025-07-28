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
} from "antd";
import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import {
  getRepairEvents,
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
import Title from "antd/es/typography/Title";
import WorkTimeTable from "./WorkTImeTable";

interface Props {
  zoneId: number;
}

const WorkTypeList = ({ zoneId }: Props) => {
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
      const res = await getRepairEvents(zoneId);
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
    } catch (err) {
      message.error("Ошибка при сохранении вида работ");
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteRepairEvent(id);
      message.success("Удалено");
      loadWorks();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  return (
    <>
      <Title level={2}>Работы на участках</Title>
      <Space style={{ marginBottom: 16 }}>
        <Button icon={<PlusOutlined />} onClick={openCreateModal}>
          Добавить вид работ
        </Button>
      </Space>

      {/* <List
        bordered
        dataSource={repairEvents}
        renderItem={(item) => (
          <List.Item
            actions={[
              <Button
                icon={<EditOutlined />}
                onClick={() => openEditModal(item)}
              >
                Редактировать
              </Button>,
              <Popconfirm
                title="Удалить?"
                okText="ОК"
                cancelText="Отмена"
                onConfirm={() => handleDelete(item.id)}
              >
                <Button danger icon={<DeleteOutlined />}>
                  Удалить
                </Button>
              </Popconfirm>,
            ]}
          >
            <div style={{ flex: 1 }}>
              <strong>{item.typeOfRepairName}</strong>
              <br />
              {item.startedAt?.toString()} — {item.endedAt?.toString()}
              <Divider style={{ margin: "12px 0" }} />
              <MaterialList repairEventId={item.id} />
            </div>
          </List.Item>
        )}
      /> */}

      <List
        bordered
        dataSource={repairEvents}
        renderItem={(item) => (
          //
          <List.Item>
            <Card
              title={item.typeOfRepairName}
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
            >
              <p style={{ fontStyle: "italic" }}>
                Дата проведения работ: {item.startedAt?.toString()} —{" "}
                {item.endedAt?.toString()}
              </p>
              <MaterialList repairEventId={item.id} />
              <Divider />
              <MediaList repairEventId={item.id} />
              <Divider />
              <WorkTimeTable
                repairEventId={item.id}
                startDate="2025-07-01"
                endDate="2025-07-07"
              />
            </Card>
          </List.Item>
        )}
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
