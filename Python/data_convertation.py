import argparse
import os
from pathlib import Path
import pandas as pd
import re

from tqdm import tqdm
from typing import Iterable


def parse_args():
    parser = argparse.ArgumentParser()
    parser.add_argument('--path', type=str)
    return parser.parse_args()


def raw_to_csv(paths: Iterable[Path], columns, description=''):
    for path in tqdm(paths, desc=description):
        data = pd.read_csv(path, sep='\|', engine='python', header=None, names=columns)
        data = data.replace('\!$', '', regex=True)
        dir_path = str(path.parent) + '/processed/'
        isExist = os.path.exists(dir_path)
        if not isExist:
            os.makedirs(dir_path)
        data.to_csv(dir_path + path.stem + '.csv')


def match(re_pattern, collection: Iterable[Path]) -> Iterable[Path]:
    return filter(lambda p: re.match(re_pattern, p.name), collection)


if __name__ == '__main__':
    args = parse_args()

    all_columns = dict(
        AxisInputs=['ActionName', 'Delta', 'RunTime'],
        ButtonInputs=["ActionName", "State", "RunTime"],
        Items=["ItemIndex", "IsEquipment", "IsAdded", "RunTime"],
        Kills=["EntityID", "EntityName", "Level", "DistanceToPlayer", "RunTime"],
        PlayerHealth=["Health", "HealthFraction", "RunTime"],
        Stats=["AverageSpeed", "DPS", "RunTime"],
        Spawns=["EntityID", "EntityName", "Level", "DistanceToPlayer", "SpawnedDirection", "RunTime"]
    )

    all_paths = [*Path(args.path).rglob('*.log')]

    axis_inputs_paths = match('^AxisInputs', all_paths)
    button_inputs_paths = match('^ButtonInputs', all_paths)
    items_paths = match('^Items', all_paths)
    kills_paths = match('^Kills', all_paths)
    player_health_paths = match('^PlayerHealth', all_paths)
    stats_paths = match('^Stats', all_paths)
    spawns_paths = match('^Spawns', all_paths)

    raw_to_csv(axis_inputs_paths, all_columns['AxisInputs'], description='AxisInputs')
    raw_to_csv(button_inputs_paths, all_columns['ButtonInputs'], description='ButtonInputs')
    raw_to_csv(items_paths, all_columns['Items'], description='Items')
    raw_to_csv(kills_paths, all_columns['Kills'], description='Kills')
    raw_to_csv(player_health_paths, all_columns['PlayerHealth'], description='PlayerHealth')
    raw_to_csv(stats_paths, all_columns['Stats'], description='Stats')
    raw_to_csv(spawns_paths, all_columns['Spawns'], description='Spawns')
