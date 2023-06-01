from pathlib import Path

import pandas as pd
import re
from typing import Iterable


class DataAggregator:
    def __init__(self):
        self.columns = []
        self.pattern = ''

    @staticmethod
    def match(re_pattern, collection: Iterable[Path]) -> Iterable[Path]:
        return filter(lambda p: re.match(re_pattern, p.name), collection)

    def prepare_data(self, root_path: str) -> pd.DataFrame:
        all_paths = Path(root_path).rglob('*.csv')
        paths = self.match(self.pattern, all_paths)

        columns = self.columns
        data = pd.DataFrame(columns=columns+['RunDate'])
        for path in paths:
            print(path)
            raw = pd.read_csv(path)
            appended = self.get_entries(raw)
            appended['RunDate'] = [path.stem[-17:] for _ in range(0, len(appended.values))]
            data = data.append(appended)
        return data

    def get_entries(self, raw: pd.DataFrame) -> pd.DataFrame:
        pass
