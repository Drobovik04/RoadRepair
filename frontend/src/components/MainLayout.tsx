import React, { useState } from "react";
import {
  DesktopOutlined,
  FileOutlined,
  PieChartOutlined,
  TeamOutlined,
  UnorderedListOutlined,
  UserOutlined,
} from "@ant-design/icons";
import type { MenuItemProps, MenuProps } from "antd";
import {
  Avatar,
  Breadcrumb,
  Button,
  Flex,
  Layout,
  Menu,
  Space,
  Splitter,
  theme,
  Typography,
} from "antd";
import { logout } from "../services/auth";
import styles from "./MainLayout.module.css";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import type { RootState } from "../store";

const { Header, Content, Footer, Sider } = Layout;

type MenuItem = Required<MenuProps>["items"][number];

function getItem(
  label: React.ReactNode,
  key: React.Key,
  icon?: React.ReactNode,
  children?: MenuItem[]
): MenuItem {
  return {
    key,
    icon,
    children,
    label,
  } as MenuItem;
}

const items: MenuItem[] = [
  getItem("Профиль", "profile", <UserOutlined />),
  getItem("Единицы измерения", "typesOfMeasure", <UnorderedListOutlined />),
  getItem("Материалы", "materials", <UnorderedListOutlined />),
  getItem("Типы услуги", "typesOfService", <UnorderedListOutlined />),
  getItem("Контрагенты", "contractors", <UnorderedListOutlined />),
  getItem("Должности", "positions", <UnorderedListOutlined />),
  getItem("Сотрудники", "workers", <UnorderedListOutlined />),
  getItem("Типы ремонтов", "typesOfRepair", <UnorderedListOutlined />),
  getItem("Ремонты", "repairs", <UnorderedListOutlined />),
];

const menuItemToPath = new Map([
  ["profile", "/profile"],
  ["typesOfMeasure", "/typesOfMeasure"],
  ["typesOfService", "/typesOfService"],
  ["typesOfRepair", "/typesOfRepair"],
  ["materials", "/materials"],
  ["contractors", "/contractors"],
  ["positions", "/positions"],
  ["workers", "/workers"],
  ["repairs", "/repairs"],
]);

const MainLayout = () => {
  const [collapsed, setCollapsed] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  const LFMNames = useSelector(
    (state: RootState) =>
      state.auth.lastName +
      " " +
      state.auth.firstName +
      " " +
      state.auth.middleName
  );

  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();

  const handleMouseClick: MenuProps["onClick"] = (e) => {
    const key = e.key;
    if (key && menuItemToPath.has(key)) {
      navigate(menuItemToPath.get(key)!);
    }
  };

  const handleLogout = async () => {
    await logout();
    localStorage.removeItem("token");
    window.location.href = "/login";
  };

  return (
    //style={{ height: "100%" }}
    <Layout style={{ minHeight: "100%" }}>
      <Sider
        collapsible
        collapsed={collapsed}
        onCollapse={(value) => setCollapsed(value)}
      >
        <div className="demo-logo-vertical" />
        <Menu
          theme="dark"
          defaultSelectedKeys={["1"]}
          mode="inline"
          items={items}
          onClick={handleMouseClick}
        />
      </Sider>
      <Layout>
        <Header
          className={styles.header}
          style={{ padding: 0, background: colorBgContainer }}
        >
          <Flex justify="space-between" gap="middle" align="center">
            <Flex gap={10} align="center">
              <Avatar
                style={{ margin: "0px 10px" }}
                size={60}
                icon={<UserOutlined />}
              ></Avatar>
              <Typography.Text style={{ fontSize: 24 }}>
                Здравствуйте, {LFMNames}
              </Typography.Text>
            </Flex>
            <Button
              type="default"
              onClick={handleLogout}
              style={{ margin: "0 16px" }}
            >
              Выйти
            </Button>
          </Flex>
        </Header>
        <Content style={{ margin: "0 16px" }}>
          <Breadcrumb
            style={{ margin: "16px 0" }}
            //items={[{ title: "User" }, { title: "Bill" }]}
          />
          <div
            style={{
              padding: 24,
              minHeight: 360,
              background: colorBgContainer,
              borderRadius: borderRadiusLG,
            }}
          >
            <Outlet />
          </div>
        </Content>
        <Footer style={{ textAlign: "center" }}>ФУТЕР ЗАМЕНИТЬ</Footer>
      </Layout>
    </Layout>
  );
};

export default MainLayout;
