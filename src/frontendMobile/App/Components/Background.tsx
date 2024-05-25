import React, {ReactNode} from 'react';
import {StyleSheet, View, ViewStyle} from 'react-native';

const Background: React.FC<{children: ReactNode; style?: ViewStyle}> = ({
  children,
  style,
}) => {
  const inputStyle = StyleSheet.compose(defaultStyle, style);

  return (
    <View style={inputStyle}>
      {children}
    </View>
  );
};

const defaultStyle: ViewStyle = {
  width: '100%',
  height: '100%',
  justifyContent: 'center',
  alignItems: 'center',
  backgroundColor: '#4C4A62'
};

export default Background;
