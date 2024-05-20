import React, {useEffect, useState} from "react";
import {Button, ColorPicker, Form, Input, List, PaginationProps} from "antd";
import ActivityTypeListItem from "../components/ActivityType/ActivityTypeListItem";
import InlineAddWithSearch from "../components/Common/InlineAddWithSearch";
import {ActivityTypesClient} from "../web-api-client";
import {IFilterProps} from "../globals/interfaces";


const client = new ActivityTypesClient();

interface IActivityType {
    id: number;
    name: string;
}

const ActivityTypes: React.FC = () => {

    const [listData, setListData] = useState<IActivityType[]>([]);
    const [filterOptions, setFilterOptions] = useState<IFilterProps>({});

    const pageSize = 1;
    const [currentPage, setCurrentPage] = useState(1);
    const [totalSize, setTotalSize] = useState(0);


    const [showForm, setShowForm] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(false);


    const [form] = Form.useForm();

    const handleSave = (values: IActivityType) => {

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
        } = await client.getActivityTypesWithPagination(filterOptions.searchTerm, currentPage, pageSize)
        await setTotalSize(totalPages ?? 1)
        await setListData(items as unknown as IActivityType[] ?? [])
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
                                 buttonText={showForm ? "Cancel" : "Add New Activity Type"} loading={loading}/>

            {showForm &&

                <Form form={form} onFinish={handleSave} layout="inline" style={{marginBottom: 16}}>
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
                    <ActivityTypeListItem item={item}/>
                )}
            />
        </>
    )
}

export default ActivityTypes;