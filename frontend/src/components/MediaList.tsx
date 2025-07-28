import React, { useEffect, useState } from "react";
import {
  Upload,
  Button,
  List,
  Image,
  message,
  Popconfirm,
  Modal,
  Form,
  Input,
  Space,
  Col,
  Row,
  Typography,
} from "antd";
import {
  UploadOutlined,
  DeleteOutlined,
  PlusOutlined,
  EditOutlined,
} from "@ant-design/icons";
import {
  getRepairEventMedia,
  addRepairEventMedia,
  updateRepairEventMedia,
  deleteRepairEventMedia,
} from "../services/repairEventMedia";
import type { RepairEventMedia } from "../types/RepairEventMedia";

interface Props {
  repairEventId: number;
}

const MediaList = ({ repairEventId }: Props) => {
  const [mediaList, setMediaList] = useState<RepairEventMedia[]>([]);
  const [modalVisible, setModalVisible] = useState(false);
  const [form] = Form.useForm();
  const [editingRepairEventMedia, setEditingRepairEventMedia] =
    useState<RepairEventMedia | null>(null);

  const { Title } = Typography;

  const loadMedia = async () => {
    try {
      const res = await getRepairEventMedia(repairEventId);
      const updatedList = await Promise.all(
        res.data.map(async (x) => {
          try {
            const fileResponse = await fetch(`uploads/${x.filePath}`);
            const blob = await fileResponse.blob();
            const objectUrl = URL.createObjectURL(blob);
            return { ...x, blobLink: objectUrl };
          } catch (e) {
            console.error("Ошибка при загрузке файла:", x.filePath, e);
            return { ...x, blobLink: "" }; // fallback
          }
        })
      );
      setMediaList(updatedList);
    } catch {
      message.error("Ошибка загрузки медиафайлов");
    }
  };

  useEffect(() => {
    loadMedia();
  }, [repairEventId]);

  useEffect(() => {
    if (editingRepairEventMedia) {
      form.setFieldsValue({
        description: editingRepairEventMedia.description,
        file: [
          {
            uid: "-1", // уникальный ID (отрицательное — локальный файл)
            name: editingRepairEventMedia.filePath.split("/").pop() || "file",
            status: "done",
            url: `/uploads/${editingRepairEventMedia.filePath}`,
          },
        ],
      });
    }
  }, [editingRepairEventMedia]);

  useEffect(() => {
    return () => {
      mediaList.forEach((x) => {
        if (x.blobLink) {
          URL.revokeObjectURL(x.blobLink);
        }
      });
    };
  }, [mediaList]);

  const openCreateModal = () => {
    form.resetFields();
    setEditingRepairEventMedia(null);
    setModalVisible(true);
  };

  const openEditModal = (event: RepairEventMedia) => {
    form.setFieldsValue(event);
    setEditingRepairEventMedia(event);
    setModalVisible(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteRepairEventMedia(id);
      message.success("Файл удален");
      loadMedia();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  const handleSubmit = async (values: any) => {
    try {
      const file = values.file?.[0]?.originFileObj;
      if (!file && !editingRepairEventMedia) {
        message.error("Файл не выбран");
        return;
      }
      const payload = {
        ...values,
        file,
        repairEventId,
      };

      if (editingRepairEventMedia) {
        await updateRepairEventMedia(editingRepairEventMedia.id, payload);
        message.success("Медиафайл обновлен");
      } else {
        await addRepairEventMedia(payload);
        message.success("Медиафайл добавлен");
      }

      form.resetFields();
      setModalVisible(false);
      setEditingRepairEventMedia(null);
      loadMedia();
    } catch (err) {
      message.error("Ошибка при сохранении медиафайла");
    }
  };

  return (
    <div style={{ marginTop: 12 }}>
      <Title level={3}>Медиафайлы</Title>
      <Space style={{ marginBottom: 16 }}>
        <Button icon={<PlusOutlined />} onClick={openCreateModal}>
          Добавить файл
        </Button>
      </Space>
      <List
        bordered
        dataSource={mediaList}
        style={{ marginTop: 12 }}
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
                title="Удалить файл?"
                okText="ОК"
                cancelText="Отмена"
                onConfirm={() => handleDelete(item.id)}
              >
                <Button icon={<DeleteOutlined />} danger>
                  Удалить
                </Button>
              </Popconfirm>,
            ]}
          >
            <div style={{ width: "100%" }}>
              <Row gutter={16}>
                <Col flex="none">
                  {!item.filePath.endsWith("mp4") ? (
                    <Image
                      src={item.blobLink}
                      width={200}
                      style={{ objectFit: "cover", borderRadius: 4 }}
                      alt="media"
                    />
                  ) : (
                    <video width={200} controls>
                      <source src={item.blobLink} type="video/mp4" />
                      Ваш браузер не поддерживает видео.
                    </video>
                  )}
                </Col>
                <Col
                  flex="auto"
                  style={{ display: "flex", alignItems: "flex-start" }}
                >
                  <Typography.Text
                    style={{
                      overflow: "hidden",
                      display: "block",
                    }}
                  >
                    Описание: {item.description || "—"}
                  </Typography.Text>
                </Col>
              </Row>
            </div>
            {/* {!item.filePath.endsWith("mp4") ? (
              <Image
                src={"uploads/" + item.filePath}
                width={200}
                style={{ maxHeight: 150, objectFit: "cover" }}
              />
            ) : (
              <video width={200} controls>
                <source src={"uploads/" + item.filePath} />
                Ваш браузер не поддерживает видео.
              </video>
            )} */}
          </List.Item>
        )}
      />
      <Modal
        title={
          editingRepairEventMedia
            ? "Редактировать медиафайл"
            : "Добавить медиафайл"
        }
        open={modalVisible}
        onCancel={() => {
          form.resetFields();
          setEditingRepairEventMedia(null);
          setModalVisible(false);
        }}
        onOk={() => form.validateFields().then(handleSubmit)}
        okText={editingRepairEventMedia ? "Сохранить" : "Добавить"}
        cancelText="Отмена"
        destroyOnClose
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="file"
            label="Файл (фото или видео)"
            valuePropName="fileList"
            getValueFromEvent={(e) => (Array.isArray(e) ? e : e?.fileList)}
            rules={[{ required: true, message: "Пожалуйста, загрузите файл" }]}
          >
            <Upload
              beforeUpload={() => false}
              accept="image/*,video/*"
              maxCount={1}
              showUploadList={{
                showPreviewIcon: true,
                showRemoveIcon: true,
              }}
            >
              <Button>Выбрать файл</Button>
            </Upload>
          </Form.Item>
          <Form.Item name="description" label="Описание">
            <Input.TextArea />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default MediaList;
