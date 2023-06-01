import argparse
from pathlib import Path
import re
from typing import Iterable

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
    axis = AxisAggregator().prepare_data(args.path)
    enemies = EnemiesAggregator().prepare_data(args.path)
    stats = StatsAggregator().prepare_data(args.path)
    health = HealthAggregator().prepare_data(args.path)
    buttons = ButtonsAggregator().prepare_data(args.path)
    items = ItemsAggregator().prepare_data(args.path)

    joined_data_frame = axis.merge(enemies, how='left', on=("RunTime", "RunDate"))
    joined_data_frame = joined_data_frame.merge(stats, how='left', on=("RunTime", "RunDate"))
    joined_data_frame = joined_data_frame.merge(health, how='left', on=("RunTime", "RunDate"))
    joined_data_frame = joined_data_frame.merge(buttons, how='left', on=("RunTime", "RunDate"))
    joined_data_frame = joined_data_frame.merge(items, how='left', on=("RunTime", "RunDate"))

    print(items)

    run_dates = joined_data_frame["RunDate"].unique()
    run_durations = joined_data_frame["RunDate"].value_counts()

    emotion_data = EmotionAggregator().prepare_data(args.path, run_dates, run_durations, '23-05-09-04-54-20')
    joined_data_frame = joined_data_frame.merge(emotion_data, how='left', on=("RunTime", "RunDate"))
    joined_data_frame.to_csv(args.path + '/final.csv')

