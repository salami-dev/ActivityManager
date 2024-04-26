import React, {useEffect, useRef, useState} from "react";
import {message, Skeleton} from "antd";

import {IEditFormProps, IEditFormRef} from "../../../globals/interfaces";
import {
    TaskResponseDto,
    TasksClient,
    UpdateTaskCommand
} from "../../../web-api-client";
import BaseTaskForm from "./BaseTaskForm";

import {ITask} from "../interfaces";
import {IActivity} from "../../Activity/interfaces";
import dayjs from "dayjs";
import {cleanObject} from "../../Utils/helpers";

const client = new TasksClient();
const fetchData = async (id: number) => {
    return await client.getTask(id);
}

const EditTaskForm = React.forwardRef<IEditFormRef, IEditFormProps>((props, ref) => {

    const {loading, setLoading, onFinished, editValue} = props;
    const formRef = useRef<IEditFormRef>(null);
    const [messageApi, contextHolder] = message.useMessage();
    const [initialValues, setInitialValues] = useState<ITask | undefined>(undefined);


    if (!editValue) {
        //     TODO: DIsplay warning with error
        console.warn("Edit ID has not been provided")
    }

    const onFinish = async (vals: any) => {
        console.log('Received values of form: ', vals);

        try {

            setLoading?.(true);

            const payload = UpdateTaskCommand.fromJS(cleanObject({
                status: {name: vals.status},
                content: vals.content,
                name: vals.name,
                tags: vals.tags.map((x: string) => ({name: x})),
                startDate: vals.dateTime[0],
                endDate: vals.dateTime[1],
                activityId: typeof vals.activityId == "number" ? vals.activityId : null
            }))

            console.log("payload ==> ", payload)


            const response = await client.updateTask(editValue ?? 0, payload);

            onFinished?.(); // Signal to parent that form is done, potentially to close the drawer

            messageApi.open({
                type: 'success',
                content: 'Task Created Successfully',
            });

            await _fetchData();


        } catch (e) {
            console.warn(e)
            messageApi.open({
                type: 'error',
                content: 'Failed to create task',
            });


        } finally {
            setLoading?.(false)
        }
    };

    const transformInitialValues = (data: TaskResponseDto): ITask => {
        return {
            id: data.id as number,
            name: data.name as string,
            content: data.content as string,
            startDate: data.startDate as Date,
            endDate: data.endDate as Date,
            status: data.status?.name,
            dateTime: [dayjs(data.startDate), dayjs(data.endDate)],
            created: data.created,
            tags: data.tags?.map(x => (x.name as string)),
            activity: data.activity as IActivity
        }
    }

    const _fetchData = async () => {

        setLoading?.(true)
        const payload = await fetchData(editValue as number);
        const transformedData = await transformInitialValues(payload)
        setInitialValues(transformedData)
        setLoading?.(false)

    }

    useEffect(() => {

        _fetchData()


    }, [editValue])


    React.useImperativeHandle(ref, () => ({
        submitForm: () => {
            formRef.current?.submitForm()
        },
    }));

    return (
        <>
            {contextHolder}

            {!loading ?
                <BaseTaskForm onFinished={onFinish} ref={formRef} loading={loading} initialValues={initialValues}/>
                : <Skeleton active/>}
        </>
    );
})

export default EditTaskForm;