import React from 'react';
import {Tag} from 'antd';
import type {SelectProps} from 'antd';


interface BaseTagProps {
    label: React.ReactNode;
    value: any;
    closable: boolean;
    onClose: () => void;
}

// Extend this props definition
interface ExtendedTagProps extends BaseTagProps {
    color?: string;  // Adding color as an optional property
}

type TagRenderProps = (props: ExtendedTagProps) => React.ReactElement;

const TagRender: TagRenderProps = (props) => {
    const {label, value, closable, onClose, color} = props;

    const onPreventMouseDown = (event: React.MouseEvent<HTMLSpanElement>) => {
        event.preventDefault();
        event.stopPropagation();
    };
    return (
        <Tag
            color={color ?? ""}
            onMouseDown={onPreventMouseDown}
            closable={closable}
            onClose={onClose}
            style={{marginInlineEnd: 4}}
        >
            {label}
        </Tag>
    );
};


export default TagRender;