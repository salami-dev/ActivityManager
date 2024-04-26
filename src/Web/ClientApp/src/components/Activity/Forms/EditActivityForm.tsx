import React, {useEffect, useRef, useState} from "react";
import {message, Skeleton} from "antd";
import dayjs from "dayjs";

import {IEditFormProps, IEditFormRef} from "../../../globals/interfaces";
import {ActivitiesClient, ActivityResponseDto, UpdateActivityCommand} from "../../../web-api-client";
import BaseActivityForm from "./BaseActivityForm";
import {IActivity} from "../interfaces";
import {cleanObject} from "../../Utils/helpers";


const client = new ActivitiesClient();

const fetchData = async (activityId: number) => {
    return await client.getActivity(activityId);
}

const EditActivityForm = React.forwardRef<IEditFormRef, IEditFormProps>((props, ref) => {

    const {loading, setLoading, onFinished, editValue} = props;
    const formRef = useRef<IEditFormRef>(null);
    const [messageApi, contextHolder] = message.useMessage();
    const [initialValues, setInitialValues] = useState<IActivity | undefined>(undefined);

    if (!editValue) {
        //     TODO: DIsplay warning with error
        console.warn("Edit ID has not been provided")
    }

    const onFinish = async (vals: any) => {
        console.log('Received values of form: ', vals);

        try {
            setLoading?.(true);

            const payload = UpdateActivityCommand.fromJS(cleanObject({
                activityType: {name: vals.activityType},
                status: {name: vals.status},
                description: vals.description,
                name: vals.name,
                tags: vals.tags.map((x: string) => ({name: x})),
                url: vals.url,
                startDate: vals.dateTime ? vals.dateTime[0] : null,
                endDate: vals.dateTime ? vals.dateTime[1] : null,
            }))

            console.log("payload ==> ", payload)

            // return
            const response = await client.updateActivity(editValue as number, payload);

            onFinished?.(); // Signal to parent that form is done, potentially to close the drawer

            messageApi.open({
                type: 'success',
                content: 'Activity Edited Successfully',
            });

            await _fetchData();

            // TODO: No need to reset form after edit
            // formRef?.current?.resetForm?.()

        } catch (e) {
            console.warn(e)
            messageApi.open({
                type: 'error',
                content: 'Failed to edit activity',
            });


        } finally {
            setLoading?.(false)
        }
    };

    const transformInitialValues = (data: ActivityResponseDto): IActivity => {
        return {
            id: data.id as number,
            name: data.name as string,
            description: data.description as string,
            url: data.url ?? null,
            startDate: data.startDate ?? null,
            endDate: data.endDate ?? null,
            status: data.status?.name,
            dateTime: [data.startDate ? dayjs(data.startDate) : null, data.endDate ? dayjs(data.endDate) : null],
            activityType: data.activityType?.name,
            created: data.created,
            tags: data.tags?.map(x => (x.name as string))
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
        // setTimeout(() => {
        //
        // }, 3000);

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
                <BaseActivityForm onFinished={onFinish} ref={formRef} loading={loading} initialValues={initialValues}/>
                : <Skeleton active/>}
        </>
    );
})

export default EditActivityForm;