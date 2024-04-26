import React from 'react';
import {Button, Descriptions} from 'antd';

import FormattedDate from "../Utils/FormattedDate";
import {ITask} from "./interfaces";


interface ActivityDescriptionProps {
    task: ITask; // This defines that 'activity' prop will be passed to this component
    editTask: (taskId: number) => void;
}

const TaskDescription: React.FC<ActivityDescriptionProps> = ({task, editTask}) => {

    return (
        <Descriptions title={task?.name ?? ""}
                      extra={<Button type="primary" onClick={() => editTask(task.id)}>Edit</Button>}>
            <Descriptions.Item label="Start Date">
                {task.startDate ? <FormattedDate date={task.startDate}/> : 'N/A'}
            </Descriptions.Item>
            <Descriptions.Item label="End Date">
                {task.endDate ? <FormattedDate date={task.endDate}/> : 'Ongoing'}
            </Descriptions.Item>
            <Descriptions.Item label="Activity">
                {task.activity?.name}
            </Descriptions.Item>
            <Descriptions.Item label="Content">{task.content}</Descriptions.Item>
        </Descriptions>
    );


}

export default TaskDescription;