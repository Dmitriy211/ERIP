import pandas as pd

from DataAggregation.DataAggregator import DataAggregator


class HealthAggregator(DataAggregator):
    def __init__(self):
        super().__init__()
        self.columns = ['RunTime', 'HealthFraction', 'HealthDelta']
        self.pattern = '^PlayerHealth'

    def get_entries(self, raw: pd.DataFrame) -> pd.DataFrame:
        health_fraction = []
        health_delta = []
        i = 0
        for row in raw.values:
            time = row[3]
            if time > i:
                for n in range(i, int(time) + 1):
                    if len(health_fraction) == 0:
                        health_fraction.append(1)
                    else:
                        health_fraction.append(health_fraction[-1])
                    health_delta.append(0)
                i = int(time) + 1

            health_fraction[i-1] = row[2]
            if len(health_delta) > 1:
                health_delta[i-1] = row[2] - health_fraction[i-2]

        data = {
            self.columns[0]: [*range(0, i)],
            self.columns[1]: health_fraction,
            self.columns[2]: health_delta
        }

        return pd.DataFrame(data)
