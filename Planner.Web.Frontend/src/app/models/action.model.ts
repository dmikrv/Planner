import {ActionState} from "./state.action.model";
import {ActionEnergy} from "./energy.action.model";
import {ContactTag} from "./contact.tag.model";
import {AreaTag} from "./area.tag.model";
import {LabelTag} from "./label.tag.model";

export interface Action {
  id: number,
  text: string,
  notes?: string,
  state: ActionState,
  isDone: boolean,
  isFocused: boolean,

  timeRequired?: number, // int minutes
  energy?: ActionEnergy,
  dueDate?: Date,

  contactTags?: ContactTag[],
  areaTags?: AreaTag[],
  labelTags?: LabelTag[],

  projectId?: number,

  // waitingContact?: ContactTag,
  // scheduledDate?: Date,

  isEdit: boolean
}

export const BaseActionColumns = [
  {
    key: 'isDone',
    type: 'isDone',
    label: '',
    colWidth: '1%',
  },
  {
    key: 'isFocused',
    type: 'isFocused',
    label: '',
    colWidth: `1%`,
  },
  {
    key: 'text',
    type: 'text',
    label: 'Text',
    colWidth: `20%`,
  },
  {
    key: 'state',
    type: 'select',
    label: 'State',
    selectItems: Object.values(ActionState).filter(o => typeof (o) === 'string'),
    colWidth: '5%',
    onlyEdit: true,
  },
  {
    key: 'timeRequired',
    type: 'select',
    label: 'Time Required',
    selectItems: [null, '5 minutes', '10 minutes', '15 minutes', '30 minutes', '45 minutes',
      '1 hour', '2 hours', '3 hours', '4 hours', '6 hours', '8 hours', 'whoa nelly!'],
    colWidth: '7%',
  },
  {
    key: 'energy',
    type: 'select',
    label: 'Energy',
    selectItems: [null, ...Object.values(ActionEnergy).filter(o => typeof (o) === 'string')],
    colWidth: '4%',
  },
  {
    key: 'dueDate',
    type: 'date',
    label: 'Due date',
    colWidth: '7%',
  },
  {
    key: 'projectId',
    type: 'projectSelect',
    label: 'Project',
    colWidth: '10%',
  },
  {
    key: 'tags',
    type: 'tags',
    label: 'Tags',
    colWidth: '30%',
  },

  {
    key: 'isEdit',
    type: 'isEdit',
    label: '',
  },
];
