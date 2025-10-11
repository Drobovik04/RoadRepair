import { Form, Input, Modal, Select, Button, message, DatePicker } from "antd";
import { useEffect, useState } from "react";
import type { ContractorService } from "../../types/ContractorService";
import dayjs from "dayjs";
import type { TypeOfService } from "../../types/TypeOfService";
import { getContractors } from "../../services/contractors";
import { normalize } from "../../utilities/dayjsStringConverter";
import type { Contractor } from "../../types/Contractor";
import { getTypesOfService } from "../../services/typesOfService";

interface Props {
  open: boolean;
  onClose: () => void;
  onSubmit: (values: any) => void;
  initialValues?: any;
}

const ContractorServiceForm = ({
  open,
  onClose,
  onSubmit,
  initialValues,
}: Props) => {
  const [form] = Form.useForm();
  const [formFields, setFormFields] = useState<any>(null);
  const [contractors, setContractors] = useState<Contractor[]>([]);
  const [typesOfService, setTypesOfServices] = useState<TypeOfService[]>([]);
  const [editingContractorService, setEditingContractorService] =
    useState<ContractorService | null>(null);

  useEffect(() => {
    if (open) {
      if (initialValues) {
        const normalized = normalize(initialValues, ["dateOfService"]);
        setFormFields(normalized);
        form.setFieldsValue(normalized);
        setEditingContractorService(initialValues);
      } else {
        form.resetFields();
        setEditingContractorService(null);
      }
    }
  }, [initialValues, open]);

  useEffect(() => {
    getContractors().then((res) => setContractors(res.data));
    getTypesOfService().then((res) => setTypesOfServices(res.data));
  }, []);

  const handleModalClose = () => {
    setFormFields(null);
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      open={open}
      title={
        initialValues
          ? "Редактировать услугу контрагента"
          : "Добавить услугу контрагента"
      }
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
        <Form.Item
          name="typeOfServiceId"
          label="Тип услуги"
          rules={[{ required: true }]}
        >
          <Select
            placeholder="Выберите тип услуги"
            showSearch
            optionFilterProp="children"
            filterOption={(input, option) =>
              String(option?.children)
                .toLowerCase()
                .includes(input.toLowerCase())
            }
          >
            {typesOfService.map((u) => (
              <Select.Option key={u.id} value={u.id}>
                {u.name}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>
        <Form.Item
          name="contractorId"
          label="Контрагент"
          rules={[{ required: true }]}
        >
          <Select
            placeholder="Выберите контрагента"
            showSearch
            optionFilterProp="children"
            filterOption={(input, option) =>
              String(option?.children)
                .toLowerCase()
                .includes(input.toLowerCase())
            }
          >
            {contractors.map((u) => (
              <Select.Option key={u.id} value={u.id}>
                {u.name}
              </Select.Option>
            ))}
          </Select>
        </Form.Item>
        <Form.Item name="price" label="Стоимость" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="description" label="Описание">
          <Input />
        </Form.Item>
        <Form.Item name="dateOfService" label="Дата оказания услуги">
          <DatePicker format="YYYY-MM-DD" />
        </Form.Item>
        <Form.Item name="workAreaId">
          <Input hidden />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default ContractorServiceForm;
