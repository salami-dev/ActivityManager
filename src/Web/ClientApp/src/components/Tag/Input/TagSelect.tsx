import React, {useEffect, useState} from 'react';
import {Select} from 'antd';
import type {SelectProps} from 'antd';
import TagRender from "./TagRender";
import {TagsClient} from "../../../web-api-client";
import {IOptionData} from "../../../globals/interfaces";
import {transformDataToOptions} from "../../Utils/helpers";
import debounce from "lodash/debounce";


type TagRenderProps = SelectProps['tagRender'];

interface ICustomSelectMultipleProps {
    value?: string[];  // selected value
    onChange?: (value: string[]) => void;  // function to handle changes
}

const fetchData = async (inputValue: string | null, pageNumber: number = 1, pageSize: number = 5) => {
    try {
        const client = new TagsClient();
        const data = await client.getTagsWithPagination(inputValue, pageNumber, pageSize);
        return data.items ?? [];
    } catch (error) {
        console.error('Error fetching data:', error);
        return [];
    }
};


const TagSelect: React.FC<ICustomSelectMultipleProps> = ({value, onChange}) => {
    const [options, setOptions] = useState<IOptionData[]>([]);

    const _fetchOptions = async (inputValue: string | null) => {
        const items = await fetchData(inputValue);
        const _options: IOptionData[] = transformDataToOptions(items);
        setOptions(_options);

        return _options;
    }

    // Define the fetchOptions function with debouncing
    const fetchOptions = debounce(async (inputValue: string) => {
        if (inputValue) {

            let _options = await _fetchOptions(inputValue);

            _options.unshift({value: inputValue, text: inputValue})
            setOptions(_options);

        } else {
            await _fetchOptions(null);
        }
    }, 500);

    useEffect(() => {
        (async () => {
            try {
                await _fetchOptions(null);

            } catch (error) {
                console.error('Error fetching data:', error);
            }
        })();

    }, []); // The empty array ensures this effect only runs once after the initial render


    const tRender: TagRenderProps = (props) => {

        const color = options.find(option => option.value === props.value)?.theme;
        return TagRender({color, ...props});
    }

    const handleChange = (newValue: string[]) => {
        console.log("Selected:", newValue);
        onChange?.(newValue);
    };

    const handleSearch = (inputValue: string) => {
        fetchOptions(inputValue);
    };

    return (
        <Select
            mode="multiple"
            tagRender={tRender}
            value={value}
            style={{width: '100%'}}
            options={options}
            onSearch={handleSearch}
            onChange={handleChange}
        />
    )
};

export default TagSelect;