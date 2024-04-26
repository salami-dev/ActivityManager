import React, {useRef} from "react";
import {message} from "antd";

import {ICreateFormRef, ICreateFormProps} from "../../../globals/interfaces";
import {ActivitiesClient, CreateActivityCommand} from "../../../web-api-client";
import BaseActivityForm from "./BaseActivityForm";


const CreateActivityForm = React.forwardRef<ICreateFormRef, ICreateFormProps>((props, ref) => {

    const {loading, setLoading, onFinished,} = props;
    const formRef = useRef<ICreateFormRef>(null);
    const [messageApi, contextHolder] = message.useMessage();

    const onFinish = async (vals: any) => {
        // console.log('Received values of form: ', vals);

        try {

            setLoading?.(true);
            const client = new ActivitiesClient();
            const payload = CreateActivityCommand.fromJS({
                activityType: {name: vals.activityType},
                status: {name: vals.status},
                description: vals.description,
                name: vals.name,
                tags: vals.tags.map((x: string) => ({name: x})),
                url: vals.url,
                startDate: vals.dateTime ? vals.dateTime[0] : null,
                endDate: vals.dateTime ? vals.dateTime[1] : null,
            })

            console.log("payload ==> ", payload)

            const response = await client.createActivity(payload);

            onFinished?.(); // Signal to parent that form is done, potentially to close the drawer

            messageApi.open({
                type: 'success',
                content: 'Activity Created Successfully',
            });
            setLoading?.(false)
            formRef?.current?.resetForm?.()

        } catch (e) {
            console.warn(e)
            messageApi.open({
                type: 'error',
                content: 'Failed to create activity',
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

            <BaseActivityForm onFinished={onFinish} ref={formRef} loading={loading}/>

        </>
    );
})

export default CreateActivityForm;