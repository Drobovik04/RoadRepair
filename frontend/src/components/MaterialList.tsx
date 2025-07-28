import React, { useEffect, useState } from "react";
import {
  Button,
  Form,
  Input,
  InputNumber,
  List,
  Modal,
  Popconfirm,
  Select,
  Space,
  message,
} from "antd";
import { DeleteOutlined, EditOutlined, PlusOutlined } from "@ant-design/icons";
import type { MaterialSpend } from "../types/MaterialSpend";
import {
  getMaterialSpends,
  addMaterialSpend,
  updateMaterialSpend,
  deleteMaterialSpend,
} from "../services/materialSpends";
import { getContractors } from "../services/contractors";
import { getMaterials } from "../services/materials";
import type { Contractor } from "../types/Contractor";
import type { Material } from "../types/Material";
import { useSelector } from "react-redux";
import type { RootState } from "../store";
import Title from "antd/es/typography/Title";

interface Props {
  repairEventId: number;
}

const MaterialList = ({ repairEventId }: Props) => {
  const [materialSpends, setMaterialSpends] = useState<MaterialSpend[]>([]);
  const [materials, setMaterials] = useState<Material[]>([]);
  const [contractors, setContractors] = useState<Contractor[]>([]);
  const [modalVisible, setModalVisible] = useState(false);
  const [form] = Form.useForm();
  const [editingMaterialSpend, setEditingMaterialSpend] =
    useState<MaterialSpend | null>(null);
  const organizationId = useSelector(
    (state: RootState) => state.auth.organizationId
  );

  useEffect(() => {
    loadMaterials();
    loadContractors();
  }, []);

  useEffect(() => {
    loadMaterialSpends();
  }, [repairEventId]);

  const loadMaterialSpends = async () => {
    try {
      const res = await getMaterialSpends(repairEventId);
      setMaterialSpends(res.data);
    } catch {
      message.error("Ошибка загрузки затрат материалов");
    }
  };

  const loadMaterials = async () => {
    if (organizationId == null) {
      message.error("Нет organizationId для загрузки материалов");
      return;
    }
    try {
      const res = await getMaterials(organizationId);
      setMaterials(res.data);
    } catch {
      message.error("Ошибка загрузки материалов");
    }
  };

  const loadContractors = async () => {
    try {
      const res = await getContractors();
      setContractors(res.data);
    } catch {
      message.error("Ошибка загрузки контрагентов");
    }
  };

  const openCreateModal = () => {
    form.resetFields();
    form.setFieldValue("repairEventId", repairEventId);
    setEditingMaterialSpend(null);
    setModalVisible(true);
  };

  const openEditModal = (item: MaterialSpend) => {
    form.setFieldsValue(item);
    setEditingMaterialSpend(item);
    setModalVisible(true);
  };

  const handleSubmit = async (values: any) => {
    try {
      const payload = {
        ...values,
        repairEventId,
      };

      if (editingMaterialSpend) {
        await updateMaterialSpend(editingMaterialSpend.id, payload);
        message.success("Затраты материала обновлены");
      } else {
        await addMaterialSpend(payload);
        message.success("Затраты материала добавлены");
      }

      setModalVisible(false);
      form.resetFields();
      setEditingMaterialSpend(null);
      loadMaterialSpends();
    } catch {
      message.error("Ошибка при сохранении материалов");
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteMaterialSpend(id);
      message.success("Удалено");
      loadMaterialSpends();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  return (
    <>
      <Title level={3}>Затраты материалов</Title>
      <Space style={{ marginBottom: 16 }}>
        <Button icon={<PlusOutlined />} onClick={openCreateModal}>
          Добавить затраты материала
        </Button>
      </Space>

      <List
        bordered
        dataSource={materialSpends}
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
              <strong>{item.materialName}</strong> - {item.contractorName}
              <br />
              Количество: {item.volume} {item.typeOfMeasureName}
              <br />
              Суммарная стоимость: {item.price}
            </div>
          </List.Item>
        )}
      />

      <Modal
        title={
          editingMaterialSpend
            ? "Редактировать затраты материала"
            : "Добавить затраты материала"
        }
        open={modalVisible}
        onCancel={() => {
          form.resetFields();
          setEditingMaterialSpend(null);
          setModalVisible(false);
        }}
        onOk={() => form.validateFields().then(handleSubmit)}
        okText={editingMaterialSpend ? "Сохранить" : "Добавить"}
        cancelText="Отмена"
        destroyOnClose
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="materialId"
            label="Материал"
            rules={[{ required: true }]}
          >
            <Select placeholder="Выберите материал">
              {materials.map((u) => (
                <Select.Option key={u.id} value={u.id}>
                  {u.name} {u.typeOfMeasureName}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item
            name="price"
            label="Общая стоимость"
            rules={[{ required: true }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="volume"
            label="Количество"
            rules={[{ required: true }]}
          >
            <Input />
          </Form.Item>
          <Form.Item name="repairEventId" hidden>
            <Input />
          </Form.Item>
          <Form.Item
            name="contractorId"
            label="Контрагент"
            rules={[{ required: true }]}
          >
            <Select placeholder="Выберите контрагента">
              {contractors.map((u) => (
                <Select.Option key={u.id} value={u.id}>
                  {u.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
};

export default MaterialList;
