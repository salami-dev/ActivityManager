import React, {useState} from 'react';
import {
    DesktopOutlined,
    FileOutlined,
    PieChartOutlined,
    TeamOutlined,
    UserOutlined,
} from '@ant-design/icons';
import type {MenuProps} from 'antd';
import {Layout, Menu, theme} from 'antd';
import {Outlet, NavLink} from "react-router-dom";

const {Header, Content, Footer, Sider} = Layout;

type MenuItem = Required<MenuProps>['items'][number];


function getItem(
    label: React.ReactNode,
    key: React.Key,
    icon?: React.ReactNode,
    children?: MenuItem[],
): MenuItem {
    return {
        key,
        icon,
        children,
        label,
    } as MenuItem;
}

const items: MenuItem[] = [
    getItem(<NavLink to={'/'}>Home</NavLink>, '1', <PieChartOutlined/>),
    getItem(<NavLink to={'tasks'}>Tasks</NavLink>, '2', <DesktopOutlined/>),
    getItem(<NavLink to={'activities'}>Activities</NavLink>, '3', <DesktopOutlined/>),
    // getItem(<NavLink to={'analytics'}>Analytics</NavLink>, '4', <PieChartOutlined/>),
    getItem('Settings', 'sub1', <UserOutlined/>, [
        getItem(<NavLink to={'activitytypes'}>ActivityTypes</NavLink>, '5'),
        getItem(<NavLink to={'statuses'}>Statuses</NavLink>, '6'),
        getItem(<NavLink to={'tags'}>Tags</NavLink>, '7'),
    ]),
];

const Root: React.FC = () => {
    const [collapsed, setCollapsed] = useState(false);
    const {
        token: {colorBgContainer, borderRadiusLG},
    } = theme.useToken();

    return (
        <Layout style={{minHeight: '100vh'}}>
            <Sider collapsible collapsed={collapsed} onCollapse={(value) => setCollapsed(value)}>
                <div className="demo-logo-vertical"/>
                <Menu theme="dark" defaultSelectedKeys={['1']} mode="inline" items={items}/>
            </Sider>
            <Layout>
                <Header style={{padding: 0, background: colorBgContainer}}/>
                <Content style={{margin: '0 16px'}}>
                    <div
                        style={{
                            padding: 24,
                            minHeight: 360,
                            background: colorBgContainer,
                            borderRadius: borderRadiusLG,
                        }}
                    >
                        <Outlet/>
                    </div>
                </Content>
                <Footer style={{textAlign: 'center'}}>
                    Salami Bashir ©{new Date().getFullYear()} Created by Salami Bashir
                </Footer>
            </Layout>
        </Layout>
    );
}

export default Root;