import pandas as pd

from DataAggregation.DataAggregator import DataAggregator

held_buttons = [
    "PrimarySkill"
]

pressed_buttons = [
    "Jump",
    "Equipment",
    "SecondarySkill",
    "UtilitySkill",
    "SpecialSkill"
]


class ButtonsAggregator(DataAggregator):
    def __init__(self):
        super().__init__()
        self.columns = [
            'RunTime',
            'PrimaryHeld',
            'JumpPressed',
            'EquipmentPressed',
            'SecondaryPressed',
            "UtilityPressed",
            "SpecialPressed"
        ]
        self.pattern = '^ButtonInputs'

    def get_entries(self, raw: pd.DataFrame) -> pd.DataFrame:
        held_time = {
            held_buttons[0]: []
        }
        held_press_time = {
            held_buttons[0]: 0
        }
        held_is_pressed = {
            held_buttons[0]: False
        }

        pressed_count = {
            pressed_buttons[0]: [],
            pressed_buttons[1]: [],
            pressed_buttons[2]: [],
            pressed_buttons[3]: [],
            pressed_buttons[4]: [],
        }

        i = 0
        for row in raw.values:
            time = row[3]
            if time > i:
                for n in range(i, int(time) + 1):
                    for key in held_buttons:
                        if len(held_time[key]) > 0 and held_is_pressed[key]:
                            held_press_time[key] = time
                            held_time[key][-1] = 1

                    held_time[held_buttons[0]].append(0)
                    for key in pressed_buttons:
                        pressed_count[key].append(0)

                i = int(time) + 1

            key = row[1]
            pressed = row[2] == "Pressed"
            if pressed and key in pressed_count.keys():
                pressed_count[key][i - 1] += 1
            elif key in held_buttons:
                if pressed:
                    held_press_time[key] = time
                    held_is_pressed[key] = True
                else:
                    held_time[key][i - 1] = time - held_press_time[key]
                    held_is_pressed[key] = False

        data = {
            self.columns[0]: [*range(0, int(max(raw['RunTime'])) + 1)],
            self.columns[1]: held_time[held_buttons[0]],
            self.columns[2]: pressed_count[pressed_buttons[0]],
            self.columns[3]: pressed_count[pressed_buttons[1]],
            self.columns[4]: pressed_count[pressed_buttons[2]],
            self.columns[5]: pressed_count[pressed_buttons[3]],
            self.columns[6]: pressed_count[pressed_buttons[4]]
        }

        return pd.DataFrame(data)
