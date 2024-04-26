import React, {useRef, useState} from 'react';
import {Button, Drawer, Space} from 'antd';
import {IBaseFormRef, IFormType} from "../../globals/interfaces";
import CreateTaskForm from "./Forms/CreateTaskForm";
import EditTaskForm from "./Forms/EditTaskForm";


interface TaskDrawerComponentProps {
    visible: boolean;
    onClose: () => void;
    formType: IFormType
    onFormSubmit?: () => void; // To indicate the form submission to the parent
    selectedItem?: number | undefined | null
}

const TaskDrawer: React.FC<TaskDrawerComponentProps> = ({visible, onClose, formType, onFormSubmit}) => {
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

        await onFormSubmit?.(); // Notify parent list component about the submission

        if (close) {
            await onClose(); // Close the drawer
        }

    };

    return (
        <>
            <Drawer
                title={formType.type === 'create' ? 'Create New Task' : 'Edit Existing Task'}
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
                    <CreateTaskForm ref={createFormRef} loading={loading} setLoading={setLoading}
                                    onFinished={handleFormSubmitted}/> :
                    <EditTaskForm ref={editFormRef} loading={loading} setLoading={setLoading}
                                  editValue={formType.editValue}
                                  onFinished={() => handleFormSubmitted(false)}/>
                }
            </Drawer>
        </>
    );
};

export default TaskDrawer;