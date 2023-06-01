from pathlib import Path
import pandas as pd
from typing import Iterable
from DataAggregation.DataAggregator import DataAggregator


class EnemiesAggregator(DataAggregator):
    def __init__(self):
        super().__init__()
        self.columns = ['RunTime', 'EnemyCount', 'KillRate']
        self.spawns_pattern = '^Spawns'
        self.kills_pattern = '^Kills'

    def prepare_data(self, root_path: str) -> pd.DataFrame:
        all_paths = [*Path(root_path).rglob('*.csv')]

        spawns_paths: Iterable[Path] = self.match(self.spawns_pattern, all_paths)
        kills_paths: Iterable[Path] = self.match(self.kills_pattern, all_paths)

        columns = self.columns
        data = pd.DataFrame(columns=columns)

        for spawns_path in spawns_paths:
            kills_path = next(path for path in kills_paths if path.stem[-17:] == spawns_path.stem[-17:])
            print(spawns_path)
            print(kills_path)
            spawns_raw = pd.read_csv(spawns_path)
            kills_raw = pd.read_csv(kills_path)
            processed = self.get_entries(spawns_raw, kills_raw)
            processed['RunDate'] = [spawns_path.stem[-17:] for i in range(0, len(processed.values))]
            data = data.append(processed)
        return data

    def get_entries(self, spawns_raw: pd.DataFrame, kills_raw: pd.DataFrame) -> pd.DataFrame:
        enemy_delta = []
        kill_rate = []

        i = 0
        for row in spawns_raw.values:
            time = row[-1]
            if time > i:  # if next second
                for n in range(i, int(time) + 1):
                    enemy_delta.append(0)
                    kill_rate.append(0)
                i = int(time) + 1
            enemy_delta[i-1] += 1

        i = 0
        for row in kills_raw.values:
            time = row[-1]
            if time > i:  # if next second
                for n in range(i, int(time) + 1):
                    if len(enemy_delta) <= int(time):
                        enemy_delta.append(0)
                    if len(kill_rate) <= int(time):
                        kill_rate.append(0)
                i = int(time) + 1
            enemy_delta[i-1] -= 1
            kill_rate[i-1] += 1

        enemy_count = []
        for i in range(0, len(enemy_delta)):
            enemy_count.append(sum(enemy_delta[0:i]))

        data = {
            self.columns[0]: [*range(0, len(enemy_count))],
            self.columns[1]: enemy_count,
            self.columns[2]: kill_rate
        }

        return pd.DataFrame(data)
