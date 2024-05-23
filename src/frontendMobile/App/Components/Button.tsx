import {
  StyleSheet,
  Text,
  TouchableOpacity,
} from 'react-native';

interface LostCardButtonProps {
  text: string;
  onPress: () => void;
}

const LostCardButton: React.FC<LostCardButtonProps> = ({onPress, text}) => {
    return (
        <TouchableOpacity style={styles.button} onPress={onPress}>
          <Text style={styles.text}>{text}</Text>
        </TouchableOpacity>
      );
};

const styles = StyleSheet.create({
    button: {
      backgroundColor: '#A6ADB0',
      paddingHorizontal: 30,
      paddingVertical: 8,
      alignSelf: 'flex-start',
      borderRadius: 2,
    },
    text: {
      fontFamily: 'Roboto',
      fontSize: 20,
      color: 'black'
    },
  });

export default LostCardButton;
