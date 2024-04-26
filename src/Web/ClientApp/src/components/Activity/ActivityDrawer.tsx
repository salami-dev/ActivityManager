import React, {useRef, useState} from 'react';
import {Button, Drawer, Space} from 'antd';
import CreateActivityForm from "./Forms/CreateActivityForm";
import EditActivityForm from "./Forms/EditActivityForm";
import {IBaseFormRef, IFormType} from "../../globals/interfaces";


interface ActivityDrawerComponentProps {
    visible: boolean;
    onClose: () => void;
    formType: IFormType
    onFormSubmit?: () => void; // To indicate the form submission to the parent
}

const ActivityDrawer: React.FC<ActivityDrawerComponentProps> = ({visible, onClose, formType, onFormSubmit}) => {
    const [loading, setLoading] = useState(false);
    const createFormRef = useRef<IBaseFormRef>(null);
    const editFormRef = useRef<IBaseFormRef>(null);

    const onSubmit = () => {
        if (formType.type === 'create' && createFormRef.current) {
            createFormRef.current.submitForm();
        }

        if (formType.type === 'edit' && editFormRef.current) {
            editFormRef.current.submitForm();
        }
    }

    const handleFormSubmitted = async (close: boolean = true) => {

        console.log("close val", close)
        await onFormSubmit?.(); // Notify parent list component about the submission

        if (close) {
            await onClose(); // Close the drawer
        }

    };

    return (
        <>
            <Drawer
                title={formType.type === 'create' ? 'Create New Activity' : 'Edit Existing Activity'}
                width={720}
                onClose={onClose}
                open={visible}
                styles={{
                    body: {
                        paddingBottom: 80,
                    },
                }}
                extra={
                    <Space>
                        <Button onClick={onClose}>Cancel</Button>
                        <Button onClick={onSubmit} type="primary" disabled={loading} loading={loading}>
                            Submit
                        </Button>
                    </Space>
                }
            >

                {formType.type === 'create' ?
                    <CreateActivityForm ref={createFormRef} loading={loading} setLoading={setLoading}
                                        onFinished={handleFormSubmitted}/> :
                    <EditActivityForm ref={editFormRef} loading={loading} setLoading={setLoading}
                                      editValue={formType.editValue}
                                      onFinished={() => handleFormSubmitted(false)}/>}
            </Drawer>
        </>
    );
};

export default ActivityDrawer;