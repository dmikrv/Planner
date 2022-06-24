import {ProjectState} from "./state.project.model";

export interface Project {
  id: number,
  name: string,
  notes?: string,
  dueDate?: Date,
  state: ProjectState

  isEdit: boolean
}

export const BaseProjectColumns = [
  {
    key: 'name',
    type: 'text',
    label: 'Name',
    colWidth: `20%`,
  },
  {
    key: 'state',
    type: 'select',
    label: 'State',
    selectItems: Object.values(ProjectState).filter(o => typeof (o) === 'string'),
    colWidth: '5%',
    onlyEdit: true,
  },
  {
    key: 'dueDate',
    type: 'date',
    label: 'Due date',
    colWidth: '10%',
  },

  {
    key: 'isEdit',
    type: 'isEdit',
    label: '',
  },
];
