import pandas as pd

from DataAggregation.DataAggregator import DataAggregator


class ItemsAggregator(DataAggregator):
    def __init__(self):
        super().__init__()
        self.columns = ['RunTime', 'ItemCount']
        self.pattern = '^Items'

    def get_entries(self, raw: pd.DataFrame) -> pd.DataFrame:
        item_count = []

        item_sum = 0
        i = 0
        for row in raw.values:
            item_sum += 1
            time = row[4]
            if time > i:
                for n in range(i, int(time) + 1):
                    if len(item_count) == 0:
                        item_count.append(0)
                    else:
                        item_count.append(item_count[-1])
                i = int(time) + 1
            item_count[i-1] = item_sum

        data = {
            self.columns[0]: [*range(0, i)],
            self.columns[1]: item_count,
        }

        return pd.DataFrame(data)
