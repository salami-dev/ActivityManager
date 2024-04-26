import React, {useEffect, useState} from "react";
import {Badge, Button, ColorPicker, List, Popover, Space, Typography} from "antd";
import {StatusClient} from "../../web-api-client";

const {Text} = Typography;

interface IStatus {
    id?: number,
    name: string,
    theme?: string | null
}

interface IStatusListItemProps {
    item: IStatus,

}

const client = new StatusClient();

const StatusListItem: React.FC<IStatusListItemProps> = ({item}) => {
    const [editingStatus, setEditingStatus] = useState<number | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [state, setState] = useState<IStatus | null>(null)

    const handleSave = async () => {
        setLoading(true)
        try {

            // setEditingStatus(null)
        } catch (e) {

        } finally {
            setTimeout(() => {
                setEditingStatus(null)
                setLoading(false)
            }, 3000)
            // setLoading(false)
        }
        // const data = client.
    }

    useEffect(() => {
        setState({id: item.id, name: item.name, theme: item.theme})
    }, [item])

    return (
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
                avatar={
                    <Badge color={state?.theme ?? ""}/>
                }
                title={<Text editable={{
                    onStart: () => (setEditingStatus(state?.id ?? null)),
                    onChange: (val) => {
                        const payload = {...state, name: val}
                        console.log("up st payload =? ", payload)
                        setState(payload)
                    }

                }}>{state?.name}</Text>}
            />
            {editingStatus === state?.id && (
                <Popover
                    content={<ColorPicker defaultValue={state.theme}
                                          onChange={(color, hex) => {
                                              setState({...state, theme: hex})
                                          }}/>}
                    title="Select Color"
                    trigger="click"
                    // visible={editingStatus === state.id}
                >
                    <Button type="link">Change Color</Button>
                </Popover>
            )}
        </List.Item>
    )
}

export default StatusListItem;