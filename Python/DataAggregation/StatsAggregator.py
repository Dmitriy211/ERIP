import pandas as pd

from DataAggregation.DataAggregator import DataAggregator


class StatsAggregator(DataAggregator):
    def __init__(self):
        super().__init__()
        self.columns = ['RunTime', 'AverageSpeed', 'DPS']
        self.pattern = '^Stats'

    def get_entries(self, raw: pd.DataFrame) -> pd.DataFrame:
        data = {
            self.columns[0]: [*range(0, len(raw.values))],
            self.columns[1]: raw['AverageSpeed'],
            self.columns[2]: raw['DPS']
        }

        return pd.DataFrame(data)
