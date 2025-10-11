import { Form, Input, Modal, Select, Button, message, DatePicker } from "antd";
import { useEffect, useState } from "react";
import dayjs from "dayjs";
import { getWorkers } from "../../services/workers";
import { normalize } from "../../utilities/dayjsStringConverter";
import type { WorkArea } from "../../types/WorkArea";
import type { Worker } from "../../types/Worker";

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (values: any) => void;
  initialValues?: any;
}

const RepairForm = ({ open, onClose, onSubmit, initialValues }: Props) => {
  const [form] = Form.useForm();
  const [formFields, setFormFields] = useState<any>(null);
  const [editingWorkArea, setEditingWorkArea] = useState<WorkArea | null>(null);
  const [workers, setWorkers] = useState<Worker[]>([]);

  useEffect(() => {
    if (open) {
      if (initialValues) {
        const normalized = normalize(initialValues, ["createdAt", "updatedAt"]);
        setFormFields(normalized);
        form.setFieldsValue(normalized);
        setEditingWorkArea(initialValues);
      } else {
        form.resetFields();
        setEditingWorkArea(null);
      }
    }
  }, [initialValues, open]);

  useEffect(() => {
    getWorkers().then((res) => setWorkers(res.data));
    //form.setFieldValue("organizationId", organizationId);
  }, []);

  const handleModalClose = () => {
    setFormFields(null);
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      open={open}
      title={initialValues ? "Редактировать ремонт" : "Добавить ремонт"}
      onCancel={handleModalClose}
      onOk={() => form.submit()}
      okText={initialValues ? "Сохранить" : "Добавить"}
      cancelText="Отмена"
      destroyOnHidden
    >
      <Form
        form={form}
        initialValues={formFields}
        onFinish={onSubmit}
        layout="vertical"
        preserve={false}
      >
        <Form.Item name="name" label="Название" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="description" label="Описание">
          <Input.TextArea />
        </Form.Item>
        <Form.Item name="createdAt" label="Создана">
          <DatePicker format="YYYY-MM-DD" />
        </Form.Item>
        <Form.Item
          name="updatedAt"
          label="Обновлено"
          hidden={initialValues ? false : true}
        >
          <DatePicker format="YYYY-MM-DD" />
        </Form.Item>

        <Form.Item name="responsibleId" label="Ответственный">
          <Select
            placeholder="Выберите ответственного"
            showSearch
            optionFilterProp="children"
            filterOption={(input, option) =>
              String(option?.children)
                .toLowerCase()
                .includes(input.toLowerCase())
            }
          >
            {workers.map((u) => (
              <Select.Option key={u.id} value={u.id}>
                {u.lastName + " " + u.firstName + " " + u.middleName}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default RepairForm;
