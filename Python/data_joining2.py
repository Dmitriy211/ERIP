import argparse
from pathlib import Path
import re
from typing import Iterable

import pandas as pd

from DataAggregation.AxisAggregator import AxisAggregator
from DataAggregation.ButtonsAggregator import ButtonsAggregator
from DataAggregation.EnemiesAggregator import EnemiesAggregator
from DataAggregation.StatsAggregator import StatsAggregator
from DataAggregation.HealthAggregator import HealthAggregator
from DataAggregation.ItemsAggregator import ItemsAggregator
from DataAggregation.EmotionAggregator import EmotionAggregator


def match(re_pattern, collection: Iterable[Path]) -> Iterable[Path]:
    return filter(lambda p: re.match(re_pattern, p.name), collection)


def parse_args():
    parser = argparse.ArgumentParser()
    parser.add_argument('--path', type=str)
    return parser.parse_args()


if __name__ == '__main__':
    args = parse_args()
    all_paths = Path(args.path).rglob('final.csv')

    combined_data = pd.DataFrame()
    i = 0
    for path in all_paths:
        data = pd.read_csv(path, index_col=0)
        data['Participant'] = [*(i for n in range(0, len(data.values)))]
        i += 1
        combined_data = pd.concat([combined_data, data], ignore_index=True)

    print(combined_data)

    combined_data.to_csv(args.path + "/Combined.csv")


