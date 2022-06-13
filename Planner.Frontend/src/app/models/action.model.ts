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
  duedate?: Date,

  contactTags?: ContactTag[],
  areaTags?: AreaTag[],
  labelTags?: LabelTag[],

  projectId?: number,

  waitingContact?: ContactTag,
  scheduledDate?: Date,

  isEdit: boolean
}

export const BaseActionColumns = [
  {
    key: 'isDone',
    type: 'isSelected',
    label: 'Done?',
  },
  {
    key: 'isFocused',
    type: 'isSelected',
    label: 'Focused?',
  },
  {
    key: 'text',
    type: 'text',
    label: 'Text'
  },
  {
    key: 'notes',
    type: 'text',
    label: 'Notes'
  },
  {
    key: 'state',
    type: 'select',
    label: 'State',
    selectItems: Object.values(ActionState).filter(o => typeof (o) === 'string')
  },
  {
    key: 'timeRequired',
    type: 'text',
    label: 'Time Required'
  },
  {
    key: 'energy',
    type: 'select',
    label: 'Energy',
    selectItems: [null, ...Object.values(ActionEnergy).filter(o => typeof (o) === 'string')]
  },
  {
    key: 'duedate',
    type: 'date',
    label: 'Due date'
  },

  {
    key: 'isEdit',
    type: 'isEdit',
    label: '',
  },
];
