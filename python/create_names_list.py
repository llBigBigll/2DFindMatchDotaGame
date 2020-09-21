from pathlib import Path
from typing import List


def get_list_of_names(path: Path, suffix: str = '.png'):
    names_list = []
    for f in path.iterdir():
        if f.suffix == suffix:
            names_list.append(f.stem)

    return names_list


def save_list_of_names(path: Path, names_list: List[str], sep: str = ' '):
    txt_data = sep.join(names_list)
    with open(path, 'w') as f:
        f.write(txt_data)


if __name__ == '__main__':
    image_path = Path(__file__ + '/..').resolve().parent / 'Assets/Resources/HeroesPics'
    names = get_list_of_names(image_path)
    print(f'Total heroes: {len(names)}')

    names_path = Path(__file__ + '/..').resolve().parent / 'Assets/Resources/heroesList.txt'
    save_list_of_names(names_path, names)
