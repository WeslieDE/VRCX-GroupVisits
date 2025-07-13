<template>
    <div class="avatar-card" :style="cardStyle">
        <img
            :src="avatar.thumbnailImageUrl"
            alt="Avatar"
            class="avatar"
            :style="imageStyle"
        />
        <div class="avatar-name">{{ avatar.name }}</div>
    </div>
</template>

<script>
export default {
    name: 'AvatarCard',
    props: {
        avatar: {
            type: Object,
            required: true
        }
    },
    computed: {
        isDarkMode() {
            return this.$root.isDarkMode;
        },
        cardStyle() {
            return {
                background: this.isDarkMode ? '#1e1e1e' : '#ffffff',
                color: this.isDarkMode ? '#ffffff' : '#000000'
            };
        },
        imageStyle() {
            return {
                backgroundColor: this.isDarkMode ? '#1e1e1e' : '#f0f0f0'
            };
        }
    }
};
</script>

<style scoped>
.avatar-card {
    border-radius: 12px;
    width: 200px;
    padding: 5px;
    box-sizing: border-box;
    box-shadow: 0 6px 16px rgba(0, 0, 0, 0.4);
    text-align: center;
}
.avatar {
    width: 190px;
    height: 190px;
    object-fit: cover;
    border-radius: 8px;
    display: block;
    margin: 0 auto;
}
.avatar-name {
    margin-top: 12px;
    font-size: 16px;
    font-weight: bold;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}
</style>
