import React, {useEffect, useState} from "react";
import {Button, List, message, Typography} from "antd";
import {ActivityTypesClient, UpdateActivityTypeCommand} from "../../web-api-client";

const {Text} = Typography;

interface IActivityType {
    id?: number,
    name: string,
}

interface IActivityTypeListItemProps {
    item: IActivityType,

}

const client = new ActivityTypesClient();

const ActivityTypeListItem: React.FC<IActivityTypeListItemProps> = ({item}) => {
    const [editingStatus, setEditingStatus] = useState<number | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [state, setState] = useState<IActivityType | null>(null)
    const [messageApi, contextHolder] = message.useMessage();

    const handleSave = async () => {
        setLoading(true)
        try {

            const response = await client.updateActivityType(
                item.id ?? 0,
                UpdateActivityTypeCommand.fromJS({
                    name: state?.name
                })
            )

            setEditingStatus(null)


            messageApi.open({
                type: 'success',
                content: 'Activity Type Edited Successfully',
            });


        } catch (e) {

            console.warn(e)
            messageApi.open({
                type: 'error',
                content: 'Failed to edit Activity Type',
            });

        } finally {
            setLoading(false)
            // setTimeout(() => {
            //     setEditingStatus(null)
            //     setLoading(false)
            // }, 3000)
            // setLoading(false)
        }
        // const data = client.
    }

    useEffect(() => {
        setState({id: item.id, name: item.name})
    }, [item])

    return (
        <>
            {contextHolder}
            <List.Item
                actions={[
                    editingStatus !== item.id && !loading && (
                        <Button type="link" onClick={() => setEditingStatus(state?.id ?? null)}>
                            Edit
                        </Button>
                    ),
                    editingStatus === item.id && (
                        <Button type="primary" disabled={loading} loading={loading}
                                onClick={() => handleSave()}>
                            Save
                        </Button>
                    )

                ]}
            >
                <List.Item.Meta
                    title={<Text editable={{
                        onStart: () => (setEditingStatus(state?.id ?? null)),
                        onChange: (val) => {
                            const payload = {...state, name: val}
                            console.log("up st payload =? ", payload)
                            setState(payload)
                        }

                    }}>{state?.name}</Text>}
                />
            </List.Item>
        </>
    )
}

export default ActivityTypeListItem;