import React, { useEffect, useState } from "react";
import { Card, List, Typography, Space, Tag, Button, message } from "antd";
import {
  DownloadOutlined,
  FileExcelOutlined,
  CalendarOutlined,
} from "@ant-design/icons";
import { getReportHistory } from "../../services/reports";

const { Title, Text } = Typography;

interface ReportHistoryItem {
  id: number;
  reportType: string;
  workAreaName: string;
  createdAt: string;
  fileName: string;
  downloadUrl?: string;
}

const ReportHistory = () => {
  const [history, setHistory] = useState<ReportHistoryItem[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    loadHistory();
  }, []);

  const loadHistory = async () => {
    setLoading(true);
    try {
      const data = await getReportHistory();
      setHistory(data);
    } catch (error) {
      message.error("Не удалось загрузить историю отчетов");
    } finally {
      setLoading(false);
    }
  };

  const handleDownload = (item: ReportHistoryItem) => {
    if (item.downloadUrl) {
      const link = document.createElement("a");
      link.href = item.downloadUrl;
      link.download = item.fileName;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    } else {
      message.warning("Файл недоступен для скачивания");
    }
  };

  const getReportTypeLabel = (type: string) => {
    const labels: { [key: string]: string } = {
      costs: "Затраты",
      workTime: "Трудозатраты",
      materials: "Материалы",
      contractors: "Подрядчики",
      summary: "Сводный",
    };
    return labels[type] || type;
  };

  const getReportTypeColor = (type: string) => {
    const colors: { [key: string]: string } = {
      costs: "red",
      workTime: "blue",
      materials: "green",
      contractors: "orange",
      summary: "purple",
    };
    return colors[type] || "default";
  };

  return (
    <Card title="История отчетов" bordered>
      <List
        loading={loading}
        dataSource={history}
        renderItem={(item) => (
          <List.Item
            actions={[
              <Button
                key="download"
                type="link"
                icon={<DownloadOutlined />}
                onClick={() => handleDownload(item)}
                disabled={!item.downloadUrl}
              >
                Скачать
              </Button>,
            ]}
          >
            <List.Item.Meta
              avatar={
                <FileExcelOutlined style={{ fontSize: 24, color: "#52c41a" }} />
              }
              title={
                <Space>
                  <Text strong>{item.workAreaName}</Text>
                  <Tag color={getReportTypeColor(item.reportType)}>
                    {getReportTypeLabel(item.reportType)}
                  </Tag>
                </Space>
              }
              description={
                <Space>
                  <CalendarOutlined />
                  <Text type="secondary">
                    {new Date(item.createdAt).toLocaleString("ru-RU")}
                  </Text>
                </Space>
              }
            />
          </List.Item>
        )}
        locale={{ emptyText: "История отчетов пуста" }}
      />
    </Card>
  );
};

export default ReportHistory;
