import React, {useEffect} from "react";
import {Button, Col, DatePicker, Form, Input, Row, Space} from "antd";
import ActivityTypeSelect from "../../ActivityType/Input/ActivityTypeSelect";
import StatusSelect from "../../Status/Input/StatusSelect";
import TagSelect from "../../Tag/Input/TagSelect";
import {selectBefore} from "../../Utils/UrlFormSelect";
import {IBaseFormRef, IBaseFormProps as _IBaseFormProps} from "../../../globals/interfaces";
import {IActivity} from "../interfaces";


interface IBaseFormProps extends _IBaseFormProps {
    initialValues?: IActivity | undefined
}

const BaseActivityForm = React.forwardRef<IBaseFormRef, IBaseFormProps>((props, ref) => {

    const {loading, onFinished, initialValues} = props;

    const [form] = Form.useForm();

    useEffect(() => {
        console.log("setting ufgb")
        if (initialValues) {
            form.setFieldsValue(initialValues)
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
                    <Col span={12}>
                        <Form.Item
                            name="name"
                            label="Name"
                            rules={[{required: true, message: 'Please enter activity name'}]}
                        >
                            <Input placeholder="Study Plan"/>
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item
                            name="url"
                            label="Url"
                            rules={[{required: false, message: 'Please enter url'}]}
                        >
                            <Input
                                style={{width: '100%'}}
                                addonBefore={selectBefore}
                                placeholder="example.com"
                            />
                        </Form.Item>
                    </Col>
                </Row>
                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item
                            name="status"
                            label="Status"
                            rules={[{required: true, message: 'Please select a status'}]}
                        >
                            <StatusSelect/>
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item
                            name="activityType"
                            label="Activity Type"
                            rules={[{required: true, message: 'Please choose the activity type'}]}
                        >
                            <ActivityTypeSelect/>

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
                            rules={[{required: false, message: 'Select a Start and/or an end date'}]}
                        >
                            <DatePicker.RangePicker
                                placeholder={['Optional Start Date', 'Optional End Date']}
                                allowEmpty={[true, true]}
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
                            name="description"
                            label="Description"
                            rules={[
                                {
                                    required: true,
                                    message: 'please enter activity description',
                                },
                            ]}
                        >
                            <Input.TextArea rows={4} placeholder="please enter activity description"/>
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

export default BaseActivityForm;