import React from 'react';
import {Button, Descriptions} from 'antd';

import FormattedDate from "../Utils/FormattedDate";
import {IActivity} from "./interfaces";


interface ActivityDescriptionProps {
    activity: IActivity; // This defines that 'activity' prop will be passed to this component
    editActivity: (activityId: number) => void;
}

const ActivityDescription: React.FC<ActivityDescriptionProps> = ({activity, editActivity}) => {

    return (
        <Descriptions title={activity.name}
                      extra={<Button type="primary" onClick={() => editActivity(activity.id)}>Edit</Button>}>
            <Descriptions.Item label="Start Date">
                {activity.startDate ? <FormattedDate date={activity.startDate}/> : 'N/A'}
            </Descriptions.Item>
            <Descriptions.Item label="End Date">
                {activity.endDate ? <FormattedDate date={activity.endDate}/> : 'Ongoing'}
            </Descriptions.Item>
            <Descriptions.Item label="URL">{activity.url ?
                <a href={activity.url}>{activity.url}</a> : ""}</Descriptions.Item>
            <Descriptions.Item label="Description">{activity.description}</Descriptions.Item>
        </Descriptions>
    );


}

export default ActivityDescription;