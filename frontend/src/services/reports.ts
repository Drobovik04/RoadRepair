import api from './axiosInstance';

export const generateMaterialsReport = async (workAreaId: number, startDate: string, endDate: string)=> {
  try {
    const response = await api.get(`Report/materialReport`, 
      {
        params: { workAreaId, startDate, endDate },
        responseType: "blob",
      });

    downoloadFileFromBlob(response.data, `MaterialsReport_${workAreaId}_${new Date().toISOString().split("T")[0]}.xlsx`);

    return {
      success: true,
      message: "Отчет успешно сгенерирован и скачан",
    };
  } catch (error: any) {
    console.error("Ошибка при генерации отчета:", error);
  }
};

export const generateServicesReport = async (workAreaId: number, startDate: string, endDate: string)=> {
  try {
    const response = await api.get(`Report/thirdPartyServicesReport`, 
      {
        params: { workAreaId, startDate, endDate },
        responseType: "blob",
      });

    downoloadFileFromBlob(response.data, `ServicesReport_${workAreaId}_${new Date().toISOString().split("T")[0]}.xlsx`);
    
    return {
      success: true,
      message: "Отчет успешно сгенерирован и скачан",
    };
  } catch (error: any) {
    console.error("Ошибка при генерации отчета:", error);
  }
};

export const generateWorkTimeReport = async (workAreaId: number, startDate: string, endDate: string)=> {
  try {
    const response = await api.get(`Report/workTimeReport`, 
      {
        params: { workAreaId, startDate, endDate },
        responseType: "blob",
      });

    downoloadFileFromBlob(response.data, `WorkTimeReport_${workAreaId}_${new Date().toISOString().split("T")[0]}.xlsx`);
    
    return {
      success: true,
      message: "Отчет успешно сгенерирован и скачан",
    };
  } catch (error: any) {
    console.error("Ошибка при генерации отчета:", error);
  }
};

export const generateAllInOneReport = async (workAreaId: number, startDate: string, endDate: string)=> {
  try {
    const response = await api.get(`Report/allInOneReport`, 
      {
        params: { workAreaId, startDate, endDate },
        responseType: "blob",
      });

    downoloadFileFromBlob(response.data, `AllInOneReport_${workAreaId}_${new Date().toISOString().split("T")[0]}.xlsx`);
    
    return {
      success: true,
      message: "Отчет успешно сгенерирован и скачан",
    };
  } catch (error: any) {
    console.error("Ошибка при генерации отчета:", error);
  }
};


// Получение истории отчетов пользователя
export const getReportHistory = async () => {
  try {
    const token = localStorage.getItem("token");
    const response = await api.get(`/Report/history`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error("Ошибка при получении истории отчетов:", error);
    throw new Error("Не удалось загрузить историю отчетов");
  }
};

const downoloadFileFromBlob = async (data: any, name: string) => {
    const blob = new Blob([data]);
    const downloadUrl = window.URL.createObjectURL(blob);

    const link = document.createElement("a");
    link.href = downloadUrl;
    link.download = name;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    window.URL.revokeObjectURL(downloadUrl);
}