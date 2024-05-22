import React, {useEffect, useState} from 'react';
import {
    List,
    Button,
    Form,
    Input,
    ColorPicker,
    PaginationProps,
} from 'antd';

import StatusListItem from "../components/Status/StatusListItem";
import {StatusClient} from "../web-api-client";
import {IFilterProps} from "../globals/interfaces";
import PaginatedListSearchForm from "../components/Utils/PaginatedListSearchForm";
import {CloseOutlined, PlusOutlined} from "@ant-design/icons";
import InlineAddWithSearch from "../components/Common/InlineAddWithSearch";


const client = new StatusClient();

interface IStatus {
    id: number;
    name: string;
    theme: string;
}

const Statuses = () => {
    const [listData, setListData] = useState<IStatus[]>([]);
    const [filterOptions, setFilterOptions] = useState<IFilterProps>({});

    const pageSize = 10;
    const [currentPage, setCurrentPage] = useState(1);
    const [totalSize, setTotalSize] = useState(0);

    const [showForm, setShowForm] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(false);

    const [form] = Form.useForm();
    const handleSave = (values: IStatus) => {
        const newStatus = {
            id: listData.length + 1,
            name: values.name,
            theme: values.theme
        };
        // setStatuses([newStatus, ...statuses]);

        setLoading(true);
        setTimeout(() => {
            setShowForm(false)
            setLoading(false);
        }, 3000)
    };


    const _fetch = async () => {
        const {
            items,
            totalPages
        } = await client.getStatusWithPagination(filterOptions.searchTerm, currentPage, pageSize)
        await setTotalSize(totalPages ?? 1)
        await setListData(items as unknown as IStatus[] ?? [])
    }

    const onChange: PaginationProps['onChange'] = async (page) => {
        await setCurrentPage(page);
    };

    useEffect(
        () => {
            (
                async () => {
                    await _fetch()
                }
            )();

        },
        [filterOptions, currentPage])

    return (
        <>

            <InlineAddWithSearch filterOptions={filterOptions} onSearch={setFilterOptions} form={form}
                                 showForm={showForm} toggleForm={setShowForm}
                                 buttonText={showForm ? "Cancel" : "Add New Status"} loading={loading}/>


            {showForm &&

                <Form form={form} onFinish={handleSave} layout="inline" style={{marginBottom: 16}}>
                    <Form.Item
                        name="theme"
                        rules={[{required: true, message: 'Please pick a color!'}]}
                    >
                        <ColorPicker showText/>
                    </Form.Item>

                    <Form.Item
                        name="name"
                        rules={[{required: true, message: 'Please input the status name!'}]}
                    >
                        <Input placeholder="Enter status name"/>
                    </Form.Item>

                    <Form.Item>
                        <Button type="primary"
                                htmlType="submit"
                                loading={loading}
                                disabled={loading}
                        >Create Status</Button>
                    </Form.Item>
                </Form>
            }

            <List
                pagination={{
                    position: 'bottom',
                    align: 'center',
                    pageSize: pageSize,
                    total: totalSize,
                    current: currentPage,
                    onChange: onChange
                }}
                dataSource={listData}
                renderItem={item => (
                    <StatusListItem item={item}/>
                )}
            />
        </>
    );
};

export default Statuses;
