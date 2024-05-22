import React, {useEffect, useState} from "react";


import {Button, Divider, Flex, FloatButton, List, PaginationProps, Space} from 'antd';


import {PlusOutlined} from "@ant-design/icons";

import {TasksClient} from "../web-api-client";
import PaginatedListSearchForm from "../components/Utils/PaginatedListSearchForm";
import {IFilterProps, IFormType} from "../globals/interfaces";
import {ITask} from "../components/Task/interfaces";
import TaskDescription from "../components/Task/TaskDescription";
import TaskDrawer from "../components/Task/TaskDrawer";


const fetchData = async (search: string | null, activityId: number | null = null, pageNumber: number = 1, pageSize: number = 10) => {
    const client = new TasksClient();
    return await client.getTasksWithPagination(search, activityId, pageNumber, pageSize);
}
const Tasks: React.FC = () => {
    const pageSize = 10;

    const [listData, setListData] = useState<ITask[]>([]);
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
        const {items, totalPages} = await fetchData(opts?.searchTerm ?? null, opts?.activityId, currentPage, pageSize);
        await setTotalSize(totalPages ?? 1)
        await setListData(items as unknown as ITask[] ?? [])

    }

    const onEditHandler = (key: number) => {
        showDrawer({type: 'edit', editValue: key})
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
                            New Task
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
                            <TaskDescription task={item} editTask={onEditHandler}/>
                        </List.Item>
                    )}
                />
            </Space>
            <FloatButton icon={<PlusOutlined/>} onClick={() => showDrawer({type: 'create'})}/>
            <TaskDrawer visible={drawerVisible} onClose={onDrawerClose} formType={formType}
                        onFormSubmit={() => handleFormSubmit(false)}/>
        </>
    );
}

export default Tasks;