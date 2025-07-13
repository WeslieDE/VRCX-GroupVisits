<template>
    <div class="avatar-card">
        <img :src="avatar.thumbnailImageUrl" alt="Avatar" class="avatar" />
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
    }
};
</script>

<style scoped>
.avatar-card {
    background: #1e1e1e;
    border-radius: 12px;
    width: 200px;
    padding: 5px;
    box-sizing: border-box;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
    text-align: center;
    color: white;
}
.avatar {
    width: 190px;
    height: 190px;
    object-fit: cover;
    border-radius: 8px;
    background-color: #1e1e1e;
    display: block;
    margin: 0 auto;
}
.avatar-name {
    margin-top: 12px;
    font-size: 16px;
    font-weight: bold;
}
</style>
