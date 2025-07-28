import {
  Button,
  Table,
  Space,
  Modal,
  Form,
  Input,
  message,
  Popconfirm,
} from "antd";
import { useEffect, useState } from "react";
import type { Contractor } from "../../types/Contractor";
import {
  getContractors,
  addContractor,
  updateContractor,
  deleteContractor,
} from "../../services/contractors";
import ContractorForm from "./ContractorForm";

const ContractorsPage = () => {
  const [contractors, setContractors] = useState<Contractor[]>([]);
  const [formVisible, setFormVisible] = useState(false);
  const [editingContractor, setEditingContractor] = useState<Contractor | null>(
    null
  );

  const loadContractors = () => {
    getContractors()
      .then((res) => setContractors(res.data))
      .catch(() => message.error("Не удалось загрузить контрагентов"));
  };

  useEffect(() => {
    loadContractors();
  }, []);

  const handleAdd = () => {
    setEditingContractor(null);
    setFormVisible(true);
  };

  const handleEdit = (record: Contractor) => {
    setEditingContractor(record);
    setFormVisible(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteContractor(id);
      message.success("Удалено");
      loadContractors();
    } catch {
      message.error("Ошибка удаления");
    }
  };

  const handleSubmit = async (values: any) => {
    try {
      const data = { ...values };
      if (editingContractor) {
        await updateContractor(editingContractor.id, data);
        message.success("Контрагент обновлен");
      } else {
        await addContractor(data);
        message.success("Контрагент добавлен");
      }
      setFormVisible(false);
      loadContractors();
    } catch {
      message.error("Ошибка при сохранении");
    }
  };

  return (
    <div>
      <Button type="primary" onClick={handleAdd} style={{ marginBottom: 16 }}>
        Добавить контрагента
      </Button>
      <Table
        rowKey="id"
        dataSource={contractors}
        bordered
        columns={[
          {
            title: "Название",
            dataIndex: "name",
            sorter: (a, b) => a.name.localeCompare(b.name),
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Адрес",
            dataIndex: "address",
            sorter: (a, b) => a.address.localeCompare(b.address),
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Адрес электронной почты",
            dataIndex: "email",
            sorter: (a, b) => {
              if (a.email == null || b.email == null) {
                return 1;
              }
              return a.email!.localeCompare(b.email!);
            },
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Контактный телефон",
            dataIndex: "contactPhone",
            sorter: (a, b) => {
              if (a.contactPhone == null || b.contactPhone == null) {
                return 1;
              }
              return a.contactPhone!.localeCompare(b.contactPhone!);
            },
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "УНП",
            dataIndex: "unp",
            sorter: (a, b) => a.unp - b.unp,
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Действия",
            render: (_, record) => (
              <div style={{ display: "flex", justifyContent: "center" }}>
                <Button onClick={() => handleEdit(record)}>Изменить</Button>
                <Popconfirm
                  title="Удалить?"
                  okText="ОК"
                  cancelText="Отмена"
                  onConfirm={() => handleDelete(record.id)}
                >
                  <Button danger style={{ marginLeft: 8 }}>
                    Удалить
                  </Button>
                </Popconfirm>
              </div>
            ),
          },
        ]}
      />
      <ContractorForm
        open={formVisible}
        onClose={() => setFormVisible(false)}
        onSubmit={handleSubmit}
        initialValues={editingContractor || undefined}
      />
    </div>
  );
};

export default ContractorsPage;
