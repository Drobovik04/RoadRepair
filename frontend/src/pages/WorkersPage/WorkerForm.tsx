import { Form, Input, Modal, Select, Button, message, DatePicker } from "antd";
import { useEffect, useState } from "react";
import type { Worker } from "../../types/Worker";
import dayjs from "dayjs";
import type { Position } from "../../types/Position";
import { getPositions } from "../../services/positions";
import { normalize } from "../../utilities/dayjsStringConverter";

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (values: any) => void;
  initialValues?: any;
}

const WorkerForm = ({ open, onClose, onSubmit, initialValues }: Props) => {
  const [form] = Form.useForm();
  const [formFields, setFormFields] = useState<any>(null);
  const [positions, setPositions] = useState<Position[]>([]);
  const [editingWorker, setEditingWorker] = useState<Worker | null>(null);

  useEffect(() => {
    if (open) {
      if (initialValues) {
        const normalized = normalize(initialValues, ["hiredAt", "firedAt"]);
        setFormFields(normalized);
        form.setFieldsValue(normalized);
        setEditingWorker(initialValues);
      } else {
        form.resetFields();
        setEditingWorker(null);
      }
    }
  }, [initialValues, open]);

  useEffect(() => {
    getPositions().then((res) => setPositions(res.data));
  }, []);

  const handleModalClose = () => {
    setFormFields(null);
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      open={open}
      title={initialValues ? "Редактировать сотрудника" : "Добавить сотрудника"}
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
        <Form.Item name="lastName" label="Фамилия" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="firstName" label="Имя" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="middleName" label="Отчество">
          <Input />
        </Form.Item>
        <Form.Item name="hiredAt" label="Нанят">
          <DatePicker format="YYYY-MM-DD" />
        </Form.Item>
        <Form.Item
          name="firedAt"
          label="Уволен"
          hidden={initialValues ? false : true}
        >
          <DatePicker format="YYYY-MM-DD" />
        </Form.Item>
        <Form.Item
          name="positionId"
          label="Должность"
          rules={[{ required: true }]}
        >
          <Select placeholder="Выберите должность">
            {positions.map((u) => (
              <Select.Option key={u.id} value={u.id}>
                {u.name}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default WorkerForm;
