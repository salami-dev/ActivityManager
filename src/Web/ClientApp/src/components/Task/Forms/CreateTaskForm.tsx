import React, {useRef} from "react";
import {message} from "antd";

import {ICreateFormRef, ICreateFormProps} from "../../../globals/interfaces";
import {CreateTaskCommand, TasksClient} from "../../../web-api-client";
import BaseTaskForm from "./BaseTaskForm";


const CreateTaskForm = React.forwardRef<ICreateFormRef, ICreateFormProps>((props, ref) => {

    const {loading, setLoading, onFinished,} = props;
    const formRef = useRef<ICreateFormRef>(null);
    const [messageApi, contextHolder] = message.useMessage();

    const onFinish = async (vals: any) => {
        console.log('Received values of form: ', vals);

        try {

            setLoading?.(true);
            const client = new TasksClient();
            const payload = CreateTaskCommand.fromJS({
                status: {name: vals.status},
                content: vals.content,
                name: vals.name,
                tags: vals.tags.map((x: string) => ({name: x})),
                startDate: vals.dateTime[0],
                endDate: vals.dateTime[1],
                activityId: vals.activityId
            })

            console.log("payload ==> ", payload)

            const response = await client.createTask(payload);

            onFinished?.(); // Signal to parent that form is done, potentially to close the drawer

            messageApi.open({
                type: 'success',
                content: 'Task Created Successfully',
            });
            setLoading?.(false)
            formRef?.current?.resetForm?.()

        } catch (e) {
            console.warn(e)
            messageApi.open({
                type: 'error',
                content: 'Failed to create task',
            });

            setLoading?.(false)
        }
    };

    React.useImperativeHandle(ref, () => ({
        submitForm: () => {
            formRef.current?.submitForm()
        },
    }));

    return (
        <>
            {contextHolder}

            <BaseTaskForm onFinished={onFinish} ref={formRef} loading={loading}/>

        </>
    );
})

export default CreateTaskForm;