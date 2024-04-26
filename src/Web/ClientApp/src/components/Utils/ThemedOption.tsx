import React from "react";

interface ThemedOptionProps {
    label: string;
    value: string;
    bgColor?: string; // Prop for background color
}

const ThemedOption: React.FC<ThemedOptionProps> = ({label, value, bgColor}) => {
    let customStyle: Record<string, string | number> = {
        padding: '8px 12px',
        borderRadius: '4px',
        display: 'inline-block',
        width: '100%',
        boxSizing: 'border-box' as const, // 'as const' is used to ensure that TypeScript infers a specific string literal type

    };

    if (bgColor) {
        customStyle["backgroundColor"] = bgColor;
        customStyle["color"] = "white";

    }

    return (
        <div style={customStyle} onMouseDown={(e) => e.preventDefault()}>{label}</div>
    );
};

export default ThemedOption;