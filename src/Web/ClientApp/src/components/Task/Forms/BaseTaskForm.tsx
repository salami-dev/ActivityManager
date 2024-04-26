import React, {useEffect} from "react";
import {Button, Col, DatePicker, Form, Input, Row, Space} from "antd";
import StatusSelect from "../../Status/Input/StatusSelect";
import TagSelect from "../../Tag/Input/TagSelect";
import {IBaseFormRef, IBaseFormProps as _IBaseFormProps} from "../../../globals/interfaces";
import {ITask} from "../interfaces";
import ActivitySelect from "../../Activity/Input/ActivitySelect";


interface IBaseFormProps extends _IBaseFormProps {
    initialValues?: ITask | undefined
}

const BaseTaskForm = React.forwardRef<IBaseFormRef, IBaseFormProps>((props, ref) => {

    const {loading, onFinished, initialValues} = props;

    const [form] = Form.useForm();

    useEffect(() => {
        console.log("setting ufgb")
        if (initialValues) {
            // initialValues.activity
            const {activity, ...iv} = initialValues;


            form.setFieldsValue(iv)
            form.setFieldValue('activityId', {value: activity?.id, label: activity?.name})

            console.log("initvalues", initialValues)
        }
    }, [form, initialValues])

    React.useImperativeHandle(ref, () => ({
        submitForm: () => {
            form.submit();
        },
        resetForm: () => {
            // form.resetFields();
        }
    }));

    return (
        <>
            <Form layout="vertical" form={form} onFinish={onFinished}>
                <Row gutter={16}>
                    <Col flex="auto">
                        <Form.Item
                            name="activityId"
                            label="Activity"
                            rules={[{required: true, message: "Please select an activity"}]}>
                            <ActivitySelect/>
                        </Form.Item>
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item
                            name="name"
                            label="Name"
                            rules={[{required: true, message: 'Please enter task name'}]}
                        >
                            <Input placeholder="Study Plan"/>
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item
                            name="status"
                            label="Status"
                            rules={[{required: true, message: 'Please select a status'}]}
                        >
                            <StatusSelect/>
                        </Form.Item>
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item
                            name="tags"
                            label="Tags"
                            rules={[{required: false, message: 'Select Tags'}]}
                        >
                            <TagSelect/>
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item
                            name="dateTime"
                            label="DateTime"
                            rules={[{required: true, message: 'Select a Start and/or an end date'}]}
                        >
                            <DatePicker.RangePicker
                                placeholder={['Start Date', 'End Date']}
                                allowEmpty={[false, false]}
                                showTime
                                style={{width: '100%'}}
                                getPopupContainer={(trigger) => trigger.parentElement!}
                            />
                        </Form.Item>
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col span={24}>
                        <Form.Item
                            name="content"
                            label="Content"
                            rules={[
                                {
                                    required: true,
                                    message: 'please enter task content',
                                },
                            ]}
                        >
                            <Input.TextArea rows={4} placeholder="Awesome Task!"/>
                        </Form.Item>
                    </Col>
                </Row>
                <Row gutter={16} justify="end">
                    <Col flex="auto">
                        <Form.Item>
                            <Space style={{display: 'flex', justifyContent: 'flex-end'}}>
                                <Button htmlType="reset" disabled={loading}>Clear</Button>
                                <Button type="primary" htmlType="submit" loading={loading} disabled={loading}>
                                    Submit
                                </Button>
                            </Space>
                        </Form.Item>
                    </Col>
                </Row>
            </Form>
        </>
    );
})

export default BaseTaskForm;