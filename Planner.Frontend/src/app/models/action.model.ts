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
}
