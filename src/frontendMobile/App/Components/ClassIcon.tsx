import { View } from "react-native";
import WarriorIcon from '../Assets/Warrior.svg'

const Warrior = () => {
  return (
    <View style={{justifyContent: 'center'}}>
      <WarriorIcon width={40} />
    </View>
  );
};

const icons = new Map<number, JSX.Element>([[15977656387767, Warrior()]]);

export default icons;