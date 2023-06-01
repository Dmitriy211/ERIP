from pathlib import Path
import pandas as pd
from typing import Iterable
from datetime import datetime

from DataAggregation.DataAggregator import DataAggregator


class EmotionAggregator(DataAggregator):
    def __init__(self):
        super().__init__()
        self.columns = ['RunTime', 'RunDate', 'Valence', 'Arousal']

    def prepare_data(self, root_path: str, run_start_dates, run_durations, rec_start_date: str) -> pd.DataFrame:
        time_format = "%y-%m-%d-%H-%M-%S"
        path = Path(root_path + '/Emotion.csv')
        raw = pd.read_csv(path)
        raw["Time"] = [i for i in range(0, len(raw.values))]

        run_start_dates_stamp = [*map(lambda x: datetime.strptime(x, time_format), run_start_dates)]
        rec_start_date_stamp = datetime.strptime(rec_start_date, time_format)

        data = {
            self.columns[0]: [],
            self.columns[1]: [],
            self.columns[2]: [],
            self.columns[3]: []
        }

        for i in range(0, len(run_start_dates)):
            run_start_index = (run_start_dates_stamp[i] - rec_start_date_stamp).seconds

            for j in range(0, run_durations[run_start_dates[i]]):
                data[self.columns[0]].append(j)
                data[self.columns[1]].append(run_start_dates[i])

                index = run_start_index + j
                if (index >= len(raw.values)):
                    data[self.columns[2]].append(0)
                    data[self.columns[3]].append(0)
                else:
                    data[self.columns[2]].append(raw['valence'][run_start_index + j])
                    data[self.columns[3]].append(raw['arousal'][run_start_index + j])


        return pd.DataFrame(data)
