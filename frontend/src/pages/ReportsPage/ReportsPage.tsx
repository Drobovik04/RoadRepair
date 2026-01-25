import { useEffect, useState } from "react";
import {
  Button,
  Card,
  DatePicker,
  Form,
  Select,
  Space,
  message,
  Row,
  Col,
  Typography,
  Divider,
  Tabs,
} from "antd";
import { DownloadOutlined, FileExcelOutlined } from "@ant-design/icons";
import type { WorkArea } from "../../types/WorkArea";
import { getWorkAreas } from "../../services/repairs";
import {
  generateAllInOneReport,
  generateMaterialsReport,
  generateServicesReport,
  generateWorkTimeReport,
} from "../../services/reports";
import ReportHistory from "./ReportHistory";
import dayjs from "dayjs";

const { RangePicker } = DatePicker;
const { Title, Text } = Typography;
const { TabPane } = Tabs;

// Типы отчетов
const REPORT_TYPES = [
  {
    value: "workTime",
    label: "Отчет по трудозатратам",
    description: "Время работы сотрудников",
  },
  {
    value: "materials",
    label: "Отчет по материалам",
    description: "Детальная информация о потраченных материалах",
  },
  {
    value: "services",
    label: "Отчет по подрядчикам",
    description: "Услуги сторонних организаций и их стоимость",
  },
  {
    value: "summary",
    label: "Сводный отчет",
    description: "Общая информация по всем аспектам ремонта",
  },
];

const ReportsPage = () => {
  const [form] = Form.useForm();
  const [workAreas, setWorkAreas] = useState<WorkArea[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchValue, setSearchValue] = useState("");

  useEffect(() => {
    loadWorkAreas();
  }, []);

  const loadWorkAreas = async () => {
    try {
      const response = await getWorkAreas();
      setWorkAreas(response.data);
    } catch (error) {
      message.error("Не удалось загрузить список ремонтов");
    }
  };

  const handleGenerateReport = async (values: any) => {
    setLoading(true);
    try {
      const { workAreaId, dateRange, reportType } = values;

      // Подготовка параметров для запроса
      const params = {
        startDate: dateRange?.[0]?.format("YYYY-MM-DD"),
        endDate: dateRange?.[1]?.format("YYYY-MM-DD"),
      };

      let result: any;

      switch (reportType) {
        case "materials": {
          result = await generateMaterialsReport(
            workAreaId,
            params.startDate,
            params.endDate,
          );
          break;
        }
        case "services": {
          result = await generateServicesReport(
            workAreaId,
            params.startDate,
            params.endDate,
          );
          break;
        }
        case "workTime": {
          result = await generateWorkTimeReport(
            workAreaId,
            params.startDate,
            params.endDate,
          );
          break;
        }
        case "summary": {
          result = await generateAllInOneReport(
            workAreaId,
            params.startDate,
            params.endDate,
          );
          break;
        }
      }

      if (result.success) {
        message.success(result.message);
      } else {
        message.error(result.message);
      }
    } catch (error: any) {
      message.error(error.message || "Ошибка при генерации отчета");
    } finally {
      setLoading(false);
    }
  };

  const selectedReportType = Form.useWatch("reportType", form);
  const selectedReport = REPORT_TYPES.find(
    (rt) => rt.value === selectedReportType,
  );

  // Фильтрация ремонтов для отображения
  const filteredWorkAreas = workAreas.filter((wa) => {
    if (!searchValue) return true;
    const searchText = searchValue.toLowerCase();
    const fullText = `${wa.name} ${wa.description || ""} ${
      wa.responsibleId ? `${wa.lastName} ${wa.firstName} ${wa.middleName}` : ""
    }`.toLowerCase();
    return fullText.includes(searchText);
  });

  return (
    <div>
      <Title level={2}>Отчеты</Title>
      <Text type="secondary">
        Генерация и управление отчетами по ремонтным работам
      </Text>

      <Divider />

      <Tabs defaultActiveKey="generate" size="large">
        <TabPane tab="Создать отчет" key="generate">
          <Row gutter={[24, 24]}>
            <Col xs={24} lg={12}>
              <Card title="Параметры отчета" bordered>
                <Form
                  form={form}
                  layout="vertical"
                  onFinish={handleGenerateReport}
                  initialValues={{
                    dateRange: [dayjs().subtract(1, "month"), dayjs()],
                  }}
                >
                  <Form.Item
                    name="reportType"
                    label="Тип отчета"
                    rules={[{ required: true, message: "Выберите тип отчета" }]}
                  >
                    <Select
                      placeholder="Выберите тип отчета"
                      options={REPORT_TYPES.map((rt) => ({
                        value: rt.value,
                        label: rt.label,
                      }))}
                    />
                  </Form.Item>

                  <Form.Item
                    name="workAreaId"
                    label="Ремонт"
                    rules={[{ required: true, message: "Выберите ремонт" }]}
                  >
                    <Select
                      placeholder="Начните вводить название ремонта..."
                      showSearch
                      onSearch={setSearchValue}
                      searchValue={searchValue}
                      allowClear
                      onClear={() => setSearchValue("")}
                      filterOption={false}
                      notFoundContent={
                        searchValue
                          ? `Ремонт не найден по запросу "${searchValue}"`
                          : "Нет доступных ремонтов"
                      }
                      style={{ width: "100%" }}
                      options={filteredWorkAreas.map((wa) => ({
                        value: wa.id,
                        label: wa.name,
                        children: (
                          <div>
                            <div style={{ fontWeight: 500 }}>{wa.name}</div>
                            {wa.description && (
                              <div
                                style={{
                                  fontSize: "12px",
                                  color: "#666",
                                  marginTop: "2px",
                                }}
                              >
                                {wa.description.length > 50
                                  ? `${wa.description.substring(0, 50)}...`
                                  : wa.description}
                              </div>
                            )}
                            {wa.responsibleId && (
                              <div
                                style={{
                                  fontSize: "12px",
                                  color: "#999",
                                  marginTop: "2px",
                                }}
                              >
                                Ответственный: {wa.lastName} {wa.firstName}{" "}
                                {wa.middleName}
                              </div>
                            )}
                          </div>
                        ),
                        key: wa.id,
                      }))}
                      dropdownStyle={{ maxHeight: 400, overflow: "auto" }}
                      optionRender={(option) => option.data.children}
                      dropdownRender={(menu) => (
                        <div>
                          {searchValue && (
                            <div
                              style={{
                                padding: "8px 12px",
                                fontSize: "12px",
                                color: "#666",
                                borderBottom: "1px solid #f0f0f0",
                              }}
                            >
                              Найдено: {filteredWorkAreas.length} из{" "}
                              {workAreas.length} ремонтов
                              <Button
                                type="link"
                                size="small"
                                onClick={() => setSearchValue("")}
                                style={{ padding: "0 4px", height: "auto" }}
                              >
                                Очистить
                              </Button>
                            </div>
                          )}
                          {menu}
                        </div>
                      )}
                    />
                  </Form.Item>

                  <Form.Item
                    name="dateRange"
                    label="Период"
                    // rules={[{ message: "Выберите период" }]}
                  >
                    <RangePicker
                      style={{ width: "100%" }}
                      format="DD.MM.YYYY"
                    />
                  </Form.Item>

                  <Form.Item>
                    <Button
                      type="primary"
                      htmlType="submit"
                      loading={loading}
                      icon={<DownloadOutlined />}
                      size="large"
                    >
                      Сгенерировать отчет
                    </Button>
                  </Form.Item>
                </Form>
              </Card>
            </Col>

            <Col xs={24} lg={12}>
              <Card title="Описание отчета" bordered>
                {selectedReport ? (
                  <div>
                    <Title level={4}>{selectedReport.label}</Title>
                    <Text>{selectedReport.description}</Text>

                    <Divider />

                    <Space direction="vertical" size="small">
                      <Text strong>Что включает отчет:</Text>
                      {selectedReport.value === "workTime" && (
                        <ul>
                          <li>Сводка по трудозатратам</li>
                        </ul>
                      )}
                      {selectedReport.value === "materials" && (
                        <ul>
                          <li>Список использованных материалов</li>
                          <li>Количество и единицы измерения</li>
                          <li>Стоимость за единицу</li>
                          <li>Общая стоимость материалов</li>
                        </ul>
                      )}
                      {selectedReport.value === "services" && (
                        <ul>
                          <li>Список подрядчиков</li>
                          <li>Выполненные услуги</li>
                          <li>Стоимость услуг</li>
                          <li>Даты выполнения</li>
                        </ul>
                      )}
                      {selectedReport.value === "summary" && (
                        <ul>
                          <li>Общая сводка по ремонту</li>
                        </ul>
                      )}
                    </Space>
                  </div>
                ) : (
                  <Text type="secondary">
                    Выберите тип отчета для просмотра описания
                  </Text>
                )}
              </Card>
            </Col>
          </Row>
        </TabPane>

        {/* <TabPane tab="История отчетов" key="history">
          <ReportHistory />
        </TabPane> */}
      </Tabs>
    </div>
  );
};

export default ReportsPage;
