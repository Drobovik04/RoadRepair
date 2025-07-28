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
import type { ContractorService } from "../../types/ContractorService";
import dayjs from "dayjs";
import {
  getContractorServices,
  addContractorService,
  updateContractorService,
  deleteContractorService,
} from "../../services/contractorService";
import ContractorServiceForm from "./ContractorServiceForm";
import { useSelector } from "react-redux";
import type { RootState } from "../../store";

interface Props {
  workAreaId: number;
}

const ContractorServicesPage = ({ workAreaId }: Props) => {
  const [contractorServices, setContractorServices] = useState<
    ContractorService[]
  >([]);
  const [formVisible, setFormVisible] = useState(false);
  const [editingContractorService, setEditingContractorService] =
    useState<ContractorService | null>(null);
  const organizationId = useSelector(
    (state: RootState) => state.auth.organizationId
  );

  const loadContractorServices = (id: number) => {
    getContractorServices(id)
      .then((res) => {
        const processed = res.data.map((item) => ({
          ...item,
          dateOfService: new Date(item.dateOfService),
        }));
        setContractorServices(processed);
        //setContractorServices(res.data);
      })
      .catch(() => message.error("Не удалось загрузить услуги контрагентов"));
  };

  useEffect(() => {
    loadContractorServices(workAreaId);
  }, [workAreaId]);

  const handleAdd = () => {
    setEditingContractorService(null);
    setFormVisible(true);
  };

  const handleEdit = (record: ContractorService) => {
    setEditingContractorService(record);
    setFormVisible(true);
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteContractorService(id);
      message.success("Удалено");
      loadContractorServices(workAreaId);
    } catch {
      message.error("Ошибка удаления");
    }
  };

  const handleSubmit = async (values: any) => {
    try {
      const data = { ...values, workAreaId };
      data.dateOfService = data.dateOfService?.format("YYYY-MM-DD");
      if (editingContractorService) {
        await updateContractorService(editingContractorService.id, data);
        message.success("Услуга контрагента обновлена");
      } else {
        await addContractorService(data);
        message.success("Услуга контрагента добавлена");
      }
      setFormVisible(false);
      loadContractorServices(workAreaId);
    } catch {
      message.error("Ошибка при сохранении");
    }
  };

  return (
    <div>
      <Button type="primary" onClick={handleAdd} style={{ marginBottom: 16 }}>
        Добавить услугу контрагента
      </Button>
      <Table
        rowKey="id"
        dataSource={contractorServices}
        bordered
        columns={[
          {
            title: "Тип услуги",
            dataIndex: "typeOfServiceName",
            sorter: (a, b) =>
              a.typeOfServiceName.localeCompare(b.typeOfServiceName),
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Контрагент",
            dataIndex: "contractorName",
            sorter: (a, b) => a.contractorName.localeCompare(b.contractorName),
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Стоимость",
            dataIndex: "price",
            sorter: (a, b) => a.price - b.price,
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Описание",
            dataIndex: "description",
            sorter: (a, b) => {
              if (a.description == null || b.description == null) {
                return 1;
              }
              return a.description!.localeCompare(b.description!);
            },
            sortDirections: ["descend", "ascend"],
          },
          {
            title: "Время оказания услуги",
            dataIndex: "dateOfService",
            sorter: (a, b) =>
              a.dateOfService.getTime() - b.dateOfService.getTime(),
            sortDirections: ["descend", "ascend"],
            render: (date: Date) => dayjs(date).format("YYYY-MM-DD"),
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
      <ContractorServiceForm
        open={formVisible}
        onClose={() => setFormVisible(false)}
        onSubmit={handleSubmit}
        initialValues={editingContractorService || undefined}
      />
    </div>
  );
};

export default ContractorServicesPage;
