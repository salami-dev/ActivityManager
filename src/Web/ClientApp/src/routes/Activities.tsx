import React, {useEffect, useState} from "react";


import {Button, Divider, Flex, FloatButton, List, PaginationProps, Space} from 'antd';

import ActivityDescription from "../components/Activity/ActivityDescription";
import {PlusOutlined} from "@ant-design/icons";
import ActivityDrawer from "../components/Activity/ActivityDrawer";
import {ActivitiesClient} from "../web-api-client";
import {IActivity} from "../components/Activity/interfaces";
import PaginatedListSearchForm from "../components/Utils/PaginatedListSearchForm";
import {IFilterProps, IFormType} from "../globals/interfaces";


const fetchData = async (search: string | null, pageNumber: number = 1, pageSize: number = 10) => {
    const client = new ActivitiesClient();
    return await client.getActivitiesWithPagination(search, pageNumber, pageSize);
}
const Activities: React.FC = () => {
    const pageSize = 10;

    const [listData, setListData] = useState<IActivity[]>([]);
    const [filterOptions, setFilterOptions] = useState<IFilterProps>({});

    const [drawerVisible, setDrawerVisible] = useState(false);
    const [formType, setFormType] = useState<IFormType>({type: 'create'});

    const [currentPage, setCurrentPage] = useState(1);
    const [totalSize, setTotalSize] = useState(0);

    const showDrawer = (fType: IFormType) => {
        setFormType(fType);
        setDrawerVisible(true);
    };

    const onDrawerClose = () => {
        setDrawerVisible(false);
    };

    const handleFormSubmit = async (close: boolean = true) => {
        // Form has been submitted, so you might want to refresh the list
        console.log('Form has been submitted. Refreshing list...');

        if (close) {
            onDrawerClose();
        }

        await _fetch(filterOptions)
    };

    const onChange: PaginationProps['onChange'] = async (page) => {
        console.log(page);
        await setCurrentPage(page);
    };

    useEffect(() => {
        (
            async () => {
                await _fetch(filterOptions)
            }
        )();
    }, [currentPage, filterOptions])

    const _fetch = async (opts: IFilterProps) => {
        // TODO: fix because serachbar wont reset page num i think

        console.log("cuurent page => ", currentPage)
        const {items, totalPages} = await fetchData(opts?.searchTerm ?? null, currentPage, pageSize);
        await setTotalSize(totalPages ?? 1)
        await setListData(items as unknown as IActivity[] ?? [])

    }

    const onEditActivity = (activityId: number) => {
        showDrawer({type: 'edit', editValue: activityId})
    }


    return (
        <>
            <Space direction="vertical" size="middle" style={{display: 'flex'}}>
                <Flex justify="space-between">
                    <Flex>
                        <PaginatedListSearchForm value={filterOptions} onChange={setFilterOptions}/>
                    </Flex>
                    <Flex align="flex-end">
                        <Button type="primary" icon={<PlusOutlined/>} onClick={() => showDrawer({type: 'create'})}>
                            New Activity
                        </Button>
                    </Flex>
                </Flex>
                <Divider/>
                <List
                    pagination={{
                        position: 'bottom',
                        align: 'center',
                        pageSize: pageSize,
                        total: totalSize,
                        current: currentPage,
                        onChange: onChange
                    }}
                    bordered
                    dataSource={listData}
                    renderItem={(item) => (
                        <List.Item>
                            <ActivityDescription activity={item} editActivity={onEditActivity}/>
                        </List.Item>
                    )}
                />
            </Space>
            <FloatButton icon={<PlusOutlined/>} onClick={() => showDrawer({type: 'create'})}/>
            <ActivityDrawer visible={drawerVisible} onClose={onDrawerClose} formType={formType}
                            onFormSubmit={() => handleFormSubmit(false)}/>
        </>
    );
}

export default Activities;