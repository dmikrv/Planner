export interface TrashAction {
  id: number,
  text: string

  isSelected: boolean,
  isEdit: boolean,
}

export const TrashActionColumns = [
  {
    key: 'isSelected',
    type: 'isSelected',
    label: '',
  },

  {
    key: 'text',
    type: 'text',
    label: 'Text',
    required: true,
  },

  {
    key: 'isEdit',
    type: 'isEdit',
    label: '',
  },
];
