import pandas as pd

from DataAggregation.DataAggregator import DataAggregator


class AxisAggregator(DataAggregator):
    def __init__(self):
        super().__init__()
        self.columns = ['RunTime', 'LookDeltaH', 'LookDeltaV']
        self.pattern = '^AxisInputs'

    def get_entries(self, raw: pd.DataFrame) -> pd.DataFrame:
        horizontal_deltas = []
        vertical_deltas = []
        i = 0
        for row in raw.values:
            time = row[3]
            if time > i:
                for n in range(i, int(time) + 1):
                    horizontal_deltas.append(0)
                    vertical_deltas.append(0)
                i = int(time) + 1

            if row[1] == 'AimHorizontalMouse':
                horizontal_deltas[i-1] += abs(row[2])
            elif row[1] == 'AimVerticalMouse':
                vertical_deltas[i-1] += abs(row[2])

        data = {
            self.columns[0]: [*range(0, i)],
            self.columns[1]: horizontal_deltas,
            self.columns[2]: vertical_deltas
        }

        return pd.DataFrame(data)
