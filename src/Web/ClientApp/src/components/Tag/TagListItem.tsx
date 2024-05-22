import React, {useEffect, useState} from "react";
import {Badge, Button, ColorPicker, List, message, Popover, Space, Typography} from "antd";
import {StatusClient, TagsClient, UpdateTagCommand} from "../../web-api-client";

const {Text} = Typography;

interface ITag {
    id?: number,
    name: string,
    theme?: string | null
}

interface IStatusListItemProps {
    item: ITag,

}

const client = new TagsClient();

const TagListItem: React.FC<IStatusListItemProps> = ({item}) => {
    const [editingStatus, setEditingStatus] = useState<number | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [state, setState] = useState<ITag | null>(null);
    const [messageApi, contextHolder] = message.useMessage();

    const handleSave = async () => {
        setLoading(true)
        try {

            const response = await client.updateTag(
                item.id ?? 0,
                UpdateTagCommand.fromJS(
                    {
                        name: state?.name,
                        theme: state?.theme
                    }
                )
            )
            setEditingStatus(null)

            messageApi.open({
                type: 'success',
                content: 'Tag Edited Successfully',
            });
        } catch (e) {

            console.warn(e)
            messageApi.open({
                type: 'error',
                content: 'Failed to edit Tag',
            });

        } finally {
            setLoading(false)
            // setTimeout(() => {
            //     setEditingStatus(null)
            //     setLoading(false)
            // }, 3000)
            // setLoading(false)

            // setEditingStatus(null)
            // setLoading(false)
        }
    }

    useEffect(() => {
        setState({id: item.id, name: item.name, theme: item.theme})
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
        </>
    )
}

export default TagListItem;